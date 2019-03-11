using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Examples {
    public class ExampleSkillTree : MonoBehaviour {
        private SkillTreeInstance _skillTree;
        
        public int skillPoints = 5;
        public SkillTreeGraph graph;
        public SkillTreePrinter printer;
        
        private void Start () {
            _skillTree = new SkillTreeInstance();
            _skillTree.Setup(graph);
            _skillTree.OnPurchase.AddListener(Purchase);
            _skillTree.OnRefund.AddListener(Refund);

            printer.Build(_skillTree);
            printer.SetPoints(skillPoints);
        }

        private void OnDestroy () {
            _skillTree.OnPurchase.RemoveListener(Purchase);
            _skillTree.OnRefund.RemoveListener(Refund);
        }

        private void Purchase () {
            skillPoints -= 1;
            printer.SetPoints(skillPoints);

            if (skillPoints <= 0) _skillTree.Root.Disable();
        }

        private void Refund () {
            skillPoints += 1;
            printer.SetPoints(skillPoints);
            
            _skillTree.Root.Enable(true);
        }
    }
}
