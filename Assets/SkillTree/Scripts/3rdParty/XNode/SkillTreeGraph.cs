using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    [CreateAssetMenu(menuName = "Dungeons And Humans/Skill Tree/Graph", fileName = "SkillTreeGraph")]
    public class SkillTreeGraph : XNode.NodeGraph {
        public SkillNodeRoot root;
    }
}
