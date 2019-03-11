using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Examples {
    public class ExampleSkillTree : MonoBehaviour {
        public SkillTreeGraph graph;
        public SkillTreePrinter printer;
        
        private void Start () {
            var skillTree = new SkillTreeInstance();
            skillTree.Setup(graph);
            
            printer.Build(skillTree);
        }
    }
}
