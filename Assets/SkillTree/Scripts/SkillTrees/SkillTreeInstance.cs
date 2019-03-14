using System.Collections.Generic;
using System.Linq;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class SkillTreeInstance {
        public NodeRoot Root { get; private set; }
        private List<INode> _skills;
        
        public UnityEvent<INode> OnPurchase { get; } = new UnityEventNode();
        public UnityEvent<INode> OnRefund { get; } = new UnityEventNode();
        
        private class UnityEventNode : UnityEvent<INode> {
        }

        public void Setup (ISkillTreeData data) {            
            Root = new NodeRoot();
            _skills = new List<INode>();

            var copy = data.GetCopy();
            foreach (var child in copy.Root.Children) {
                RecursiveAdd(child, copy.Root, Root.Children);
            }
            
            Root.Purchase();
        }

        private void RecursiveAdd (ISkillNode node, ISkillNode parent, List<INode> siblings) {
            if (node.Hide) return;
            
            if (node.IsGroup) {
                BuildGroup(node, parent, siblings);
                return;
            }

            BuildSkill(node, parent, siblings);
        }

        private void BuildGroup (ISkillNode node, ISkillNode parent, ICollection<INode> siblings) {
            var group = new NodeGroup();
            siblings.Add(group);

            if (node.Children != null) {
                foreach (var child in node.Children) {
                    RecursiveAdd(child, parent, group.Children);
                }
            }
            
            var emptyChildren = new List<ISkillNode>();
            GetEmptyChildren(node.Children, emptyChildren);
            var bestEmptyChild = emptyChildren.Find(c => c.IsPurchased) ?? emptyChildren[0];

            if (node.GroupExit != null) {
                foreach (var child in node.GroupExit) {
                    RecursiveAdd(child, bestEmptyChild, group.GroupExit);
                }
            }
            
            group.BindChildrenToExit();
        }
        
        private static void GetEmptyChildren (List<ISkillNode> list, List<ISkillNode> output) {
            foreach (var node in list) {
                if (node.Children == null || node.Children.Count == 0) {
                    output.Add(node);
                    continue;
                }
                
                GetEmptyChildren(node.Children, output);
            }
        }
        
        private void BuildSkill (ISkillNode node, ISkillNode parent, ICollection<INode> siblings) {
            if (node.IsPurchased && !parent.IsPurchased) node.IsPurchased = false;

            var skill = new NodeSkill {
                Id = node.Id,
                DisplayName = node.DisplayName,
                Graphic = node.Graphic,
                Description = node.Description,
                SkillType = node is AbilityNode ? SkillType.Ability : SkillType.Skill,
            };

            skill.OnPurchase.AddListener(() => OnPurchase.Invoke(skill));
            skill.OnRefund.AddListener(() => OnRefund.Invoke(skill));
            skill.OnParentRefund.AddListener(isPurchased => {
                if (isPurchased) OnRefund.Invoke(skill);
            });

            _skills.Add(skill);
            siblings.Add(skill);

            if (node.Children != null) {
                foreach (var child in node.Children) {
                    RecursiveAdd(child, node, skill.Children);
                }
            }
            
            if (node.IsPurchased) {
                skill.Purchase();
            }
        }

        public List<SkillSave> Save () {
            return _skills
                .Select(skill => new SkillSave {
                    id = skill.Id,
                    purchased = skill.IsPurchased,
                })
                .ToList();
        }

        public void Load (ISkillTreeData data, IEnumerable<SkillSave> save) {
            Setup(data);

            var idToSave = save.ToDictionary(s => s.id, s => s);
            foreach (var skill in _skills) {
                if (!idToSave.TryGetValue(skill.Id, out var details)) continue;

                if (details.purchased) {
                    if (!skill.IsPurchased) skill.Purchase();
                } else {
                    if (skill.IsPurchased) skill.Refund();
                }
            }
        }
    }
}
