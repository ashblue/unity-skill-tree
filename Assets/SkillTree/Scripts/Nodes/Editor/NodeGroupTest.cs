using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes.Editors {
    public class NodeGroupTest {
        public class AddChildMethod {
            [Test]
            public void It_should_add_a_child () {
                var group = new NodeGroup();
                
                group.AddChild(Substitute.For<INode>());
                
                Assert.AreEqual(1, group.Children.Count);
            }
        }

        public class EnableMethod {
            [Test]
            public void It_should_call_Enable_on_children () {
                var group = new NodeGroup();
                var child = Substitute.For<INode>();
                group.AddChild(child);

                group.Enable(SkillType.Ability, true);
                
                child.Received(1).Enable(SkillType.Ability, true);
            }
        }

        public class ParentPurchased {
            [Test]
            public void It_should_call_ParentPurchased_on_children () {
                var group = new NodeGroup();
                var child = Substitute.For<INode>();
                group.AddChild(child);
                
                group.ParentPurchased();
                
                child.Received(1).ParentPurchased();
            }
        }
    }
}