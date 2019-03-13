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
            
            RecursiveAdd(Root, data.GetCopy().Root);
            Root.Purchase();
        }

        private void RecursiveAdd (INode pointer, ISkillNode parent) {
            if (parent.Children == null) return;

            foreach (var child in parent.Children) {
                if (child.Hide) continue;

                if (child.IsGroup) {
                    var group = BuildGroup(child);      
                    pointer.AddChild(group);
                    
                    foreach (var exitChild in child.GroupExit) {
                        if (exitChild.IsGroup) {
                            var groupChild = BuildGroup(exitChild);      
                            group.GroupExit.Add(groupChild);
                        } else {
                            var skill = BuildSkill(child, exitChild);             
                            group.GroupExit.Add(skill);
                        }
                    }
                } else {
                    var skill = BuildSkill(parent, child);             
                    pointer.AddChild(skill);
                }
            }
        }

        private INode BuildGroup (ISkillNode node) {
            var group = new NodeGroup();
            RecursiveAdd(group, node);

            return group;
        }

        private INode BuildSkill (ISkillNode parent, ISkillNode child) {
            if (child.IsPurchased && !parent.IsPurchased) child.IsPurchased = false;

            var node = new NodeSkill {
                Id = child.Id,
                DisplayName = child.DisplayName,
                Graphic = child.Graphic,
                Description = child.Description,
                SkillType = child is AbilityNode ? SkillType.Ability : SkillType.Skill,
            };

            node.OnPurchase.AddListener(() => OnPurchase.Invoke(node));
            node.OnRefund.AddListener(() => OnRefund.Invoke(node));
            node.OnParentRefund.AddListener(isPurchased => {
                if (isPurchased) OnRefund.Invoke(node);
            });

            _skills.Add(node);
            RecursiveAdd(node, child);

            if (child.IsPurchased) {
                node.Purchase();
            }

            return node;
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
                    skill.Purchase();
                } else {
                    skill.Refund();
                }
            }
        }
    }
}
