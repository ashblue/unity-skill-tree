using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class GroupPrint : MonoBehaviour, ISkillPrint {
        public RectTransform childOutput;
        public RectTransform exitOutput;
        public LayoutGroup alignment;

        public TextAnchor Alignment => alignment.childAlignment;
    }
}