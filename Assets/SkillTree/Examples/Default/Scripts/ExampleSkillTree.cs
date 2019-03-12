using System.Collections.Generic;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Examples {
    public class ExampleSkillTree : MonoBehaviour {
        public SkillTreeInstance skillTree;
        
        public int skillPoints = 5;
        public SkillTreeGraph graph;
        public SkillTreePrinter printer;

        public class SaveTree {
            public int skillPoints;
            public List<SkillSave> tree;
        }
        
        private void Start () {
            Setup(null);
        }

        private void Setup (List<SkillSave> save) {
            skillTree = new SkillTreeInstance();
            if (save == null) {
                skillTree.Setup(graph);                
            } else {
                skillTree.Load(graph, save);
            }
            
            skillTree.OnPurchase.AddListener(Purchase);
            skillTree.OnRefund.AddListener(Refund);

            printer.Build(skillTree);
            printer.SetPoints(skillPoints);
        }

        private void OnDestroy () {
            skillTree.OnPurchase.RemoveListener(Purchase);
            skillTree.OnRefund.RemoveListener(Refund);
        }

        private void Purchase () {
            skillPoints -= 1;
            printer.SetPoints(skillPoints);

            if (skillPoints <= 0) skillTree.Root.Disable();
        }

        private void Refund () {
            skillPoints += 1;
            printer.SetPoints(skillPoints);
            
            skillTree.Root.Enable(true);
        }

        public SaveTree Save () {
            return new SaveTree {
                tree = skillTree.Save(),
                skillPoints = skillPoints
            };
        }

        public void Load (SaveTree save) {
            skillPoints = save.skillPoints;
            Setup(save.tree);
        }
    }
}
