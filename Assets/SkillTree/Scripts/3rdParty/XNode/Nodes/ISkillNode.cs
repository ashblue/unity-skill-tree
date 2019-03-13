using System.Collections.Generic;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    public interface ISkillNode {
        string Id { get; }
        string DisplayName { get; }
        Sprite Graphic { get; }
        bool Hide { get; }
        bool IsPurchased { get; set; }

        List<ISkillNode> Children { get; }
        List<ISkillNode> GroupExit { get; }
        string Description { get; }
        bool IsGroup { get; }
    }
}
