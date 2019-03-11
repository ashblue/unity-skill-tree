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
        void ParentRefund ();

        UnityEvent OnPurchase { get; }
        UnityEvent OnParentPurchase { get; }
        UnityEvent OnRefund { get; }
        UnityEvent<bool> OnParentRefund { get; }
    }
}