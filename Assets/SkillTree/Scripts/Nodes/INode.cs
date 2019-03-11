using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public interface INode {
        List<INode> Children { get; }
        bool IsPurchased { get; }
        bool IsEnabled { get; }
        string Id { get; set; }
        string DisplayName { get; set; }
        Sprite Graphic { get; set; }
        void AddChild (INode node);
        void Purchase ();
        void Refund ();
        void ParentPurchased();
    }
}