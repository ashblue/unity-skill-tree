using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    public interface ISkillNode {
        string Id { get; }
        string DisplayName { get; }
        Sprite Graphic { get; }
        
        bool IsPurchased { get; }
        
        List<ISkillNode> Children { get; }
    }
}
