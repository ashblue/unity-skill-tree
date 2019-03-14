using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class GroupPrint : MonoBehaviour, ISkillPrint {
        public RectTransform childOutput;
        public RectTransform exitOutput;
        public LayoutGroup alignment;
        public RectTransform siblingDivider;

        public TextAnchor Alignment => alignment.childAlignment;

        public void Setup (INode parent) {
            siblingDivider.gameObject.SetActive(parent.Children.Count > 1);
        }
    }
}