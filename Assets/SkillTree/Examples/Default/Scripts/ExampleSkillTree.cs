using System.Collections.Generic;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Examples {
    public class ExampleSkillTree : MonoBehaviour {
        private const int ABILITY_POINTS = 5;
        private int _abilityPoints = ABILITY_POINTS;

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
            
            // Catch purchases as they are registered
            skillTree.OnPurchase.AddListener(PurchaseAbility);
            skillTree.OnRefund.AddListener(RefundAbility);

            if (save == null) {
                skillTree.Setup(graph);                
            } else {
                skillTree.Load(graph, save);
            }
            
            // Post setup hooks
            skillTree.OnPurchase.AddListener(PurchaseSkill);
            skillTree.OnRefund.AddListener(RefundSkill);

            printer.Build(skillTree);
            printer.SetPoints(_abilityPoints, skillPoints);
            
            if (_abilityPoints <= 0) skillTree.Root.Disable(SkillType.Ability);
            if (skillPoints <= 0) skillTree.Root.Disable(SkillType.Skill);
        }

        private void PurchaseAbility (INode node) {
            if (node.SkillType != SkillType.Ability) return;
            
            _abilityPoints -= 1;
            if (_abilityPoints <= 0) skillTree.Root.Disable(SkillType.Ability);
            printer.SetPoints(_abilityPoints, skillPoints);
        }

        private void PurchaseSkill (INode node) {
            if (node.SkillType != SkillType.Skill) return;

            skillPoints -= 1;
            if (skillPoints <= 0) skillTree.Root.Disable(SkillType.Skill);
            printer.SetPoints(_abilityPoints, skillPoints);
        }

        private void OnDestroy () {
            skillTree.OnPurchase.RemoveListener(PurchaseAbility);
            skillTree.OnPurchase.RemoveListener(PurchaseSkill);
            skillTree.OnRefund.RemoveListener(RefundSkill);
            skillTree.OnRefund.RemoveListener(RefundAbility);
        }
        
        private void RefundAbility (INode node) {
            if (node.SkillType != SkillType.Ability) return;
            
            _abilityPoints += 1;
            skillTree.Root.Enable(SkillType.Ability, true);
            
            printer.SetPoints(_abilityPoints, skillPoints);
        }

        private void RefundSkill (INode node) {
            if (node.SkillType != SkillType.Skill) return;

            skillPoints += 1;
            skillTree.Root.Enable(SkillType.Skill, true);
            
            printer.SetPoints(_abilityPoints, skillPoints);
        }

        public SaveTree Save () {
            return new SaveTree {
                tree = skillTree.Save(),
                skillPoints = skillPoints,
            };
        }

        public void Load (SaveTree save) {
            skillPoints = save.skillPoints;
            _abilityPoints = ABILITY_POINTS;
            Setup(save.tree);
        }
    }
}
