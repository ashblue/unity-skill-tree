using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class SkillTreePrinter : MonoBehaviour {
        [Tooltip("Where nodes will be output in a Canvas")]
        [SerializeField]
        private RectTransform _nodeOutput;

        [SerializeField]
        private SkillPrint _skillPrefab;

        [SerializeField] 
        private ContextPrint _context;

        public void Build (SkillTreeInstance tree) {
            RecursivePrint(tree.Root, _nodeOutput);
        }

        private void RecursivePrint (INode node, RectTransform parent) {
            foreach (var child in node.Children) {
                var skill = Instantiate(_skillPrefab, parent);
                skill.Setup(child, node);
                skill.button.onClick.AddListener(() => _context.Open(child));
                RecursivePrint(child, skill.childOutput);
            }
        }
    }
}
