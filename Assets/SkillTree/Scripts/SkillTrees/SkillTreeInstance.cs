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
        
        public UnityEvent OnPurchase { get; } = new UnityEvent();
        public UnityEvent OnRefund { get; } = new UnityEvent();

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

                if (child.IsPurchased && !parent.IsPurchased) child.IsPurchased = false;
                
                var node = new NodeSkill {
                    Id = child.Id,
                    DisplayName = child.DisplayName,
                    Graphic = child.Graphic,
                    Description = child.Description,
                    SkillType = child is AbilityNode ? SkillType.Ability : SkillType.Skill,
                };
                
                node.OnPurchase.AddListener(OnPurchase.Invoke);
                node.OnRefund.AddListener(OnRefund.Invoke);
                node.OnParentRefund.AddListener(isPurchased => {
                    if (isPurchased) OnRefund.Invoke();
                });
                
                _skills.Add(node);
                pointer.AddChild(node);                
                RecursiveAdd(node, child);
                
                if (child.IsPurchased) {
                    node.Purchase();
                } 
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
                    skill.Purchase();
                } else {
                    skill.Refund();
                }
            }
        }
    }
}
