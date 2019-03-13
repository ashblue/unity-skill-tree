using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public class NodeRoot : INode {
        public List<INode> Children { get; } = new List<INode>();
        public List<INode> GroupExit => null;
        public bool IsPurchased => true;
        public string Id { get; } = null;
        public string DisplayName { get; } = null;
        public Sprite Graphic { get; } = null;
        public string Description { get; } = null;

        public bool IsEnabled => true;

        public UnityEvent OnPurchase { get; } = null;
        public UnityEvent OnParentPurchase { get; } = null;
        public UnityEvent OnRefund { get; } = null;
        public UnityEvent<bool> OnParentRefund { get; } = null;
        public UnityEvent OnDisable { get; } = null;
        public SkillType SkillType { get; }

        public void ParentRefund () {
            throw new System.NotImplementedException();
        }

        public void AddChild (INode node) {
            Children.Add(node);
        }

        public void Purchase () {
            foreach (var child in Children) {
                child.ParentPurchased();
            }
        }

        public void Refund () {
            throw new System.NotImplementedException();
        }

        public void ParentPurchased() {
            throw new System.NotImplementedException();
        }

        public void Disable (SkillType type) {
            foreach (var child in Children) {
                child.Disable(type);
            }
        }

        public void Enable (SkillType type, bool parentIsPurchased) {
            foreach (var child in Children) {
                child.Enable(type, true);
            }
        }
    }
}