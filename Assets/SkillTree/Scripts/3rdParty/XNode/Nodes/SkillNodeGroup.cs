namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    [NodeTint(0f, 1f, 1f)]
    [CreateNodeMenuAttribute("Skill Tree/Group")]
    public class SkillNodeGroup : SkillNodeBase {
        [Input]
        public Connection enter;
        
        [Output] 
        public Connection exit;
    }
}