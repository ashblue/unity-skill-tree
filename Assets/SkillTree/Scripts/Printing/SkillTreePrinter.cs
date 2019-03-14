using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class SkillTreePrinter : MonoBehaviour {
        [Tooltip("Where nodes will be output in a Canvas")]
        [SerializeField]
        private RectTransform _nodeOutput;

        [SerializeField]
        private SkillPrint _skillPrefab;

        [SerializeField]
        private SkillPrint _abilityPrefab;

        [SerializeField]
        private GroupPrint _groupPrefab;
        
        [SerializeField] 
        private ContextPrint _context;

        [SerializeField] 
        private Text _pointOutput;

        public void Build (SkillTreeInstance tree) {
            foreach (Transform child in _nodeOutput) {
                Destroy(child.gameObject);
            }

            foreach (var child in tree.Root.Children) {
                RecursivePrint(child, tree.Root, _nodeOutput, null, false);
            }
        }

        private void RecursivePrint (INode node, INode parent, RectTransform container, ISkillPrint nodeParent, bool parentIsGroup) {
            if (node is NodeGroup) {
                PrintGroup(node, parent, container);
                return;
            }
            
            PrintNode(node, parent, container, nodeParent, parentIsGroup);
        }

        private void PrintGroup (INode node, INode parent, RectTransform container) {
            var group = Instantiate(_groupPrefab, container);
            group.Setup(parent);
            
            foreach (var child in node.Children) {
                RecursivePrint(child, node, group.childOutput, group, true);
            }
            
            foreach (var child in node.GroupExit) {
                RecursivePrint(child, node, group.exitOutput, group, false);
            }
        }

        private void PrintNode (INode node, INode parent, RectTransform parentContainer, ISkillPrint nodeParent, bool parentIsGroup) {
            var nodePrefab = _skillPrefab;
            if (node.SkillType == SkillType.Ability) nodePrefab = _abilityPrefab;

            var skill = Instantiate(nodePrefab, parentContainer);
            skill.Setup(node, parent, nodeParent, parentIsGroup);
            skill.button.onClick.AddListener(() => _context.Open(node));
            
            foreach (var child in node.Children) {
                RecursivePrint(child, node, skill.childOutput, skill, parentIsGroup);
            }
        }

        public void SetPoints (int abilityPoints, int skillPoints) {
            _pointOutput.text = $"Ability Points: {abilityPoints}; Skill Points: {skillPoints}";
        }
    }
}
