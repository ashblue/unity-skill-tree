using System.Collections.Generic;
using System.Linq;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
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
            
            RecursiveAdd(Root, data.Root.Children);
            Root.Purchase();
        }

        private void RecursiveAdd (INode pointer, IEnumerable<ISkillNode> children) {
            if (children == null) return;
            
            foreach (var data in children) {
                var node = new NodeSkill {
                    Id = data.Id,
                    DisplayName = data.DisplayName,
                    Graphic = data.Graphic,
                    Description = data.Description,
                };
                
                node.OnPurchase.AddListener(OnPurchase.Invoke);
                node.OnRefund.AddListener(OnRefund.Invoke);
                node.OnParentRefund.AddListener(isPurchased => {
                    if (isPurchased) OnRefund.Invoke();
                });
                
                _skills.Add(node);
                pointer.AddChild(node);                
                RecursiveAdd(node, data.Children);
                
                if (data.IsPurchased) {
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
