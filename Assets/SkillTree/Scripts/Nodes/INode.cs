using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public interface INode {
        List<INode> Children { get; }
        List<INode> GroupExit { get; }
        string Id { get; }
        string DisplayName { get; }
        Sprite Graphic { get; }
        string Description { get; }
        SkillType SkillType { get; }
        
        bool IsPurchased { get; }
        bool IsEnabled { get; }
        
        UnityEvent OnPurchaseBefore { get; }
        UnityEvent OnPurchase { get; }
        UnityEvent OnParentPurchase { get; }
        UnityEvent OnRefund { get; }
        UnityEvent<bool> OnParentRefund { get; }
        UnityEvent OnDisable { get; }
        UnityEvent<SkillType, bool> OnEnable { get; }

        void AddChild (INode node);
        void Purchase ();
        void Refund ();
        void ParentPurchased();
        void ParentRefund ();
        void Disable (SkillType type);
        void Enable (SkillType type, bool parentIsPurchased);
    }
}