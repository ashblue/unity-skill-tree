using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    /// <summary>
    /// For displaying skill nodes that require data input
    /// </summary>
    public abstract class SkillNodeDisplayBase : SkillNodeBase {
        [Input(connectionType = ConnectionType.Override)]
        public Connection enter;
        
        [SerializeField] private string _displayName = "Untitled";
        
        [TextArea]
        [SerializeField] 
        private string _description = "Please provide a description";
        
        [SerializeField] 
        private Sprite _graphic;
        
        [SerializeField] 
        private bool _purchased;

        [Tooltip("Hidden nodes will be excluded at runtime from the skill tree")]
        [SerializeField]
        private bool _hide;
        
        public override string DisplayName => _displayName;
        public override string Description => _description;
        public override Sprite Graphic => _graphic;

        public override bool IsPurchased {
            get => _purchased;
            set => _purchased = value;
        }
        
        public override bool Hide => _hide;
    }
}