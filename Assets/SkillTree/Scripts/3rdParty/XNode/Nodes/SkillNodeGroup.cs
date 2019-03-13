using System.Collections.Generic;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    [NodeTint(0f, 1f, 1f)]
    [CreateNodeMenuAttribute("Skill Tree/Group")]
    public class SkillNodeGroup : SkillNodeBase {
        [Input]
        public Connection enter;
        
        [Output] 
        public Connection exit;

        public override bool IsGroup => true;
        
        public override List<ISkillNode> GroupExit {
            get {
                var port = GetOutputPort("exit");
                var list = new List<ISkillNode>();
                for (var i = 0; i < port.ConnectionCount; i++) {
                    list.Add(port.GetConnection(i).node as ISkillNode);
                }
    
                return list;
            }
        }
    }
}