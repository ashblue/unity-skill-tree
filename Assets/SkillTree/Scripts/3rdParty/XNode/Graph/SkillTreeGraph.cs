using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    [CreateAssetMenu(menuName = "Dungeons And Humans/Skill Tree/Graph", fileName = "SkillTreeGraph")]
    public class SkillTreeGraph : XNode.NodeGraph, ISkillTreeData {
        [SerializeField]
        private SkillNodeRoot _root;
        
        public ISkillNode Root => _root;
    }
}
