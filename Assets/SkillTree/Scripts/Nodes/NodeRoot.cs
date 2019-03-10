using System.Collections.Generic;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public class NodeRoot : INode {
        public List<INode> Children { get; } = new List<INode>();
        public bool IsPurchased => true;

        public void AddChild (INode node) {
            Children.Add(node);
        }
    }
}