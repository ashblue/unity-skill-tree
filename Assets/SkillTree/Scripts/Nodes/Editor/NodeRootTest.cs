using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes.Editors {
    public class NodeRootTest {
        public class AddChildMethod {
            [Test]
            public void It_should_add_a_child_to_Children () {
                var root = new NodeRoot();
                var child = Substitute.For<INode>();

                root.AddChild(child);
                
                Assert.IsTrue(root.Children.Contains(child));
            }
        }

        public class PurchaseMethod {
            [Test]
            public void It_should_call_Purchase_on_each_child() {
                var root = new NodeRoot();
                var child = Substitute.For<INode>();

                root.AddChild(child);
                root.Purchase();
                
                child.Received(1).ParentPurchased();
            }
        }
    }
}