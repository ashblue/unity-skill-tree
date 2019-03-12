using System;
using System.Collections.Generic;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Examples {
    public class ExampleSkillTree : MonoBehaviour {
        private const int ABILITY_POINTS = 5;
        
        public SkillTreeInstance skillTree;
        
        public int skillPoints = 5;
        public int abilityPoints = ABILITY_POINTS;
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
            
            // Catch purchases as they are registered
            skillTree.OnPurchase.AddListener(PurchaseAbility);
            
            if (save == null) {
                skillTree.Setup(graph);                
            } else {
                skillTree.Load(graph, save);
            }
            
            // Post setup hooks
            skillTree.OnPurchase.AddListener(PurchaseSkill);
            skillTree.OnRefund.AddListener(Refund);

            printer.Build(skillTree);
            printer.SetPoints(abilityPoints, skillPoints);
            
            if (abilityPoints <= 0) skillTree.Root.Disable(SkillType.Ability);
            if (skillPoints <= 0) skillTree.Root.Disable(SkillType.Skill);
        }

        private void PurchaseAbility (INode node) {
            if (node.SkillType != SkillType.Ability) return;
            
            abilityPoints -= 1;
            if (abilityPoints <= 0) skillTree.Root.Disable(SkillType.Ability);
            printer.SetPoints(abilityPoints, skillPoints);
        }

        private void PurchaseSkill (INode node) {
            if (node.SkillType != SkillType.Skill) return;

            skillPoints -= 1;
            if (skillPoints <= 0) skillTree.Root.Disable(SkillType.Skill);
            printer.SetPoints(abilityPoints, skillPoints);
        }

        private void OnDestroy () {
            skillTree.OnPurchase.RemoveListener(PurchaseAbility);
            skillTree.OnPurchase.RemoveListener(PurchaseSkill);
            skillTree.OnRefund.RemoveListener(Refund);
        }

        private void Refund (INode node) {
            switch (node.SkillType) {
                case SkillType.Skill: {
                    skillPoints += 1;
                    skillTree.Root.Enable(SkillType.Skill, true);
                    break;
                }
                case SkillType.Ability: {
                    abilityPoints += 1;
                    skillTree.Root.Enable(SkillType.Ability, true);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            printer.SetPoints(abilityPoints, skillPoints);
        }

        public SaveTree Save () {
            return new SaveTree {
                tree = skillTree.Save(),
                skillPoints = skillPoints,
            };
        }

        public void Load (SaveTree save) {
            skillPoints = save.skillPoints;
            abilityPoints = ABILITY_POINTS;
            Setup(save.tree);
        }
    }
}
