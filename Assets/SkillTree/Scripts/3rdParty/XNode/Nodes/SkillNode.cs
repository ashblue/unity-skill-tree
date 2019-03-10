using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    [CreateNodeMenuAttribute("Skill Tree/Skill")]
    public class SkillNode : SkillNodeBase {
        [Input(connectionType = ConnectionType.Override)]
        public Connection enter;
        
        [SerializeField] private string _displayName = "Untitled";
        [SerializeField] private Sprite _graphic;
        [SerializeField] private bool _purchased;
        
        public override string DisplayName => _displayName;
        public override Sprite Graphic => _graphic;
        public override bool IsPurchased => _purchased;
    }
}