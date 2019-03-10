namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    [CreateNodeMenuAttribute("Skill Tree/Root")]
    [NodeTint(0f, 1f, 0f)]
    public class SkillNodeRoot : SkillNodeBase {
        public override bool IsPurchased => true;
    }
}