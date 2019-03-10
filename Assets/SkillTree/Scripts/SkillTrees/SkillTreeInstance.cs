using System.Collections.Generic;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class SkillTreeInstance {
        public NodeRoot Root { get; } = new NodeRoot();

        public void Setup (ISkillTreeData data) {
            RecursiveAdd(Root, data.Root.Children);
        }

        private void RecursiveAdd (INode pointer, IEnumerable<ISkillNode> children) {
            if (children == null) return;
            
            foreach (var data in children) {
                var node = new NodeSkill {
                    IsPurchased = data.IsPurchased,
                    Id = data.Id,
                };
                
                pointer.AddChild(node);
                RecursiveAdd(node, data.Children);
            }
        }
    }
}
