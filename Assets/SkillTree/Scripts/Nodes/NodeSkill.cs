using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public class NodeSkill : INode {
        public List<INode> Children { get; } = new List<INode>();
        public bool IsPurchased { get; private set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public Sprite Graphic { get; set; }

        public void AddChild (INode node) {
            Children.Add(node);
        }

        public void Purchase () {
            IsPurchased = true;
        }

        public void Refund () {
            IsPurchased = false;
        }
    }
}
