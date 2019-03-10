using System.Collections.Generic;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public class NodeRoot : INode {
        public List<INode> Children { get; } = new List<INode>();
        public bool IsPurchased => true;
        public string Id { get; set; }

        public void AddChild (INode node) {
            Children.Add(node);
        }

        public void Purchase () {
            throw new System.NotImplementedException();
        }

        public void Refund () {
            throw new System.NotImplementedException();
        }
    }
}