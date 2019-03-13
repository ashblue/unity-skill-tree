using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public class NodeGroup : INode {
        private readonly List<INode> _exits = new List<INode>();
        
        public List<INode> Children { get; } = new List<INode>();
        public List<INode> GroupExit { get; } = new List<INode>();
        public string Id => null;
        public string DisplayName => null;
        public Sprite Graphic => null;
        public string Description => null;
        public SkillType SkillType { get; }
        public bool IsPurchased => true;
        public bool IsEnabled => true;
        
        public UnityEvent OnPurchaseBefore { get; }
        public UnityEvent OnPurchase { get; }
        public UnityEvent OnParentPurchase { get; }
        public UnityEvent OnRefund { get; }
        public UnityEvent<bool> OnParentRefund { get; }
        public UnityEvent OnDisable { get; }
        
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
            foreach (var child in Children) {
                child.ParentRefund();
            }
        }

        public void Disable (SkillType type) {
            foreach (var child in Children) {
                child.Disable(type);
            }

            foreach (var child in GroupExit) {
                child.Disable(type);
            }
        }

        public void Enable (SkillType type, bool parentIsPurchased) {
            foreach (var child in Children) {
                child.Enable(type, parentIsPurchased);
            }
        }

        /// <summary>
        /// Post setup hook that should be called to bind the last children to the exit
        /// </summary>
        public void BindChildrenToExit () {
            BindEmptyChildrenToExit(Children);
        }
        
        private void BindEmptyChildrenToExit (List<INode> list) {
            foreach (var node in list) {
                if (node.Children == null || node.Children.Count == 0) {
                    GroupExit.ForEach(n => {
                        node.OnPurchaseBefore.AddListener(n.ParentPurchased);
                        node.OnParentRefund.AddListener(i => n.ParentRefund());

                        node.OnRefund.AddListener(() => {
                            if (!_exits.Any(exit => exit.IsPurchased)) {
                                n.ParentRefund();
                            }
                        });
                    });
                    
                    _exits.Add(node);
                    continue;
                }

                BindEmptyChildrenToExit(node.Children);
            }
        }
    }
}