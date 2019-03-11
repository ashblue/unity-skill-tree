using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public interface INode {
        List<INode> Children { get; }
        bool IsPurchased { get; }
        bool IsEnabled { get; }
        string Id { get; }
        string DisplayName { get; }
        Sprite Graphic { get; }
        string Description { get; }
        
        void AddChild (INode node);
        void Purchase ();
        void Refund ();
        void ParentPurchased();
        
        UnityEvent OnPurchase { get; }
        UnityEvent OnParentPurchase { get; }
    }
}