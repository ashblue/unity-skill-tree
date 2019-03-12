using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    [CreateAssetMenu(menuName = "Dungeons And Humans/Skill Tree/Graph", fileName = "SkillTreeGraph")]
    public class SkillTreeGraph : XNode.NodeGraph, ISkillTreeData {
        [SerializeField]
        private SkillNodeRoot _root;
        
        public ISkillNode Root {
            get => _root;
            private set => _root = value as SkillNodeRoot;
        }

        public ISkillTreeData GetCopy () {
            var copy = Copy() as SkillTreeGraph;
            copy.Root = copy.nodes.Find(n => n is SkillNodeRoot) as SkillNodeRoot;

            return copy;
        }
    }
}
