using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public class NodeGroup : INode {
        public List<INode> Children { get; } = new List<INode>();
        public List<INode> GroupExit { get; } = new List<INode>();
        public string Id => null;
        public string DisplayName => null;
        public Sprite Graphic => null;
        public string Description => null;
        public SkillType SkillType { get; }
        public bool IsPurchased => true;
        public bool IsEnabled => true;
        
        public void AddChild (INode node) {
            Children.Add(node);
        }

        public void Purchase () {
            throw new System.NotImplementedException();
        }

        public void Refund () {
            throw new System.NotImplementedException();
        }

        public void ParentPurchased () {
            foreach (var child in Children) {
                child.ParentPurchased();
            }
        }

        public void ParentRefund () {
            throw new System.NotImplementedException();
        }

        public void Disable (SkillType type) {
            throw new System.NotImplementedException();
        }

        public void Enable (SkillType type, bool parentIsPurchased) {
            foreach (var child in Children) {
                child.Enable(type, parentIsPurchased);
            }
        }

        public UnityEvent OnPurchase { get; }
        public UnityEvent OnParentPurchase { get; }
        public UnityEvent OnRefund { get; }
        public UnityEvent<bool> OnParentRefund { get; }
        public UnityEvent OnDisable { get; }
    }
}