using System;
using System.Collections.Generic;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Examples {
    public class ExampleSkillTree : MonoBehaviour {
        public SkillTreeInstance skillTree;
        
        public int skillPoints = 5;
        public int abilityPoints = 5;
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
            printer.SetPoints(abilityPoints, skillPoints);
        }

        private void OnDestroy () {
            skillTree.OnPurchase.RemoveListener(Purchase);
            skillTree.OnRefund.RemoveListener(Refund);
        }

        private void Purchase (INode node) {
            switch (node.SkillType) {
                case SkillType.Skill: {
                    skillPoints -= 1;
                    if (skillPoints <= 0) skillTree.Root.Disable();
                    break;
                }
                case SkillType.Ability: {
                    abilityPoints -= 1;
                    if (abilityPoints <= 0) skillTree.Root.Disable();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            printer.SetPoints(abilityPoints, skillPoints);
        }

        private void Refund (INode node) {
            switch (node.SkillType) {
                case SkillType.Skill: {
                    skillPoints += 1;
                    break;
                }
                case SkillType.Ability: {
                    abilityPoints += 1;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            printer.SetPoints(abilityPoints, skillPoints);
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
