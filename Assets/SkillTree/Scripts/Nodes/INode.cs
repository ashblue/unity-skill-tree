using System.Collections.Generic;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public interface INode {
        List<INode> Children { get; }
        bool IsPurchased { get; }
        string Id { get; set; }
        void AddChild (INode node);
    }
}