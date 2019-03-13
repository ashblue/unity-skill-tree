using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public class NodeSkill : INode {
        public List<INode> Children { get; } = new List<INode>();
        public List<INode> GroupExit => null;
        public bool IsPurchased { get; private set; }
        public bool IsEnabled { get; private set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public Sprite Graphic { get; set; }
        public string Description { get; set; }

        public UnityEvent OnPurchaseBefore { get; } = new UnityEvent();
        public UnityEvent OnPurchase { get; } = new UnityEvent();
        public UnityEvent OnParentPurchase { get; } = new UnityEvent();
        public UnityEvent OnRefund { get; } = new UnityEvent();
        public UnityEvent OnDisable { get; } = new UnityEvent();
        public UnityEvent<SkillType, bool> OnEnable { get; } = new UnityEventOnEnable();
        public SkillType SkillType { get; set; }
        public UnityEvent<bool> OnParentRefund { get; } = new UnityEventBool();

        private class UnityEventBool : UnityEvent<bool> {}
        private class UnityEventOnEnable : UnityEvent<SkillType, bool> {}

        public void AddChild (INode node) {
            Children.Add(node);
        }

        public void Purchase () {
            IsPurchased = true;
            
            OnPurchaseBefore.Invoke();
            
            foreach (var child in Children) {
                child.ParentPurchased();
            }
            
            OnPurchase.Invoke();
        }
        
        public void ParentPurchased () {
            IsEnabled = true;
            OnParentPurchase.Invoke();
        }

        public void Refund () {
            IsPurchased = false;
            
            foreach (var child in Children) {
                child.ParentRefund();
            }
            
            OnRefund.Invoke();
        }
        
        public void ParentRefund () {
            var oldPurchase = IsPurchased;
            
            IsEnabled = false;
            IsPurchased = false;
            
            foreach (var child in Children) {
                child.ParentRefund();
            }
            
            OnParentRefund.Invoke(oldPurchase);
        }
        
        public void Disable (SkillType type) {
            if (type == SkillType && !IsPurchased) IsEnabled = false;

            foreach (var child in Children) {
                child.Disable(type);
            }

            if (type == SkillType) {
                OnDisable.Invoke();                
            }
        }
        
        public void Enable (SkillType type, bool parentIsPurchased) {
            if (parentIsPurchased && type == SkillType) ParentPurchased();
            
            foreach (var child in Children) {
                child.Enable(type, IsPurchased);
            }
            
            OnEnable.Invoke(type, parentIsPurchased);
        }
    }
}
