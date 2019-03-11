using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class SkillPrint : MonoBehaviour {
        private Color _normalColor;
        
        public Text title;
        public Image graphic;
        public Image purchaseGraphic;
        public Button button;
        public LayoutGroup alignment;

        public RectTransform childOutput;
        public RectTransform connectorLeft;
        public RectTransform connectorRight;

        private void Awake() {
            _normalColor = button.colors.normalColor;
        }

        public void Setup (INode child, INode parent) {
            title.text = child.DisplayName;
            graphic.sprite = child.Graphic;
            purchaseGraphic.gameObject.SetActive(child.IsPurchased);
            
            connectorLeft.gameObject.SetActive(!(parent is NodeRoot));
            connectorRight.gameObject.SetActive(child.Children.Count > 0);

            if (!(parent is NodeRoot) && parent.Children.Count > 0) {
                AdjustAlignment(child, parent);
            }

            if (child.IsEnabled) {
                ButtonEnable();
            } else {
                ButtonDisable();
            }
        }

        private void AdjustAlignment (INode child, INode parent) {
            if (parent.Children[0] == child) {
                alignment.childAlignment = TextAnchor.UpperLeft;
            } else if (parent.Children[parent.Children.Count - 1] == child) {
                alignment.childAlignment = TextAnchor.LowerLeft;
            }
        }

        private void ButtonDisable () {
            var colors = button.colors;
            colors.normalColor = colors.disabledColor;

            button.colors = colors;
        }

        private void ButtonEnable () {
            var colors = button.colors;
            colors.normalColor = _normalColor;

            button.colors = colors;
        }
    }
}
