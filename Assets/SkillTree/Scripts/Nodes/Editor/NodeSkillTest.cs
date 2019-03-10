using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes.Editors {
    public class NodeSkillTest {
        private NodeSkill _node;
        
        [SetUp]
        public void BeforeEach () {
            _node = new NodeSkill();
        }
        
        public class AddChildMethod : NodeSkillTest {
            [Test]
            public void It_should_add_a_child_to_Children () {
                var child = Substitute.For<INode>();

                _node.AddChild(child);
                
                Assert.IsTrue(_node.Children.Contains(child));
            }
        }

        public class IsPurchasedProperty : NodeSkillTest {
            [Test]
            public void It_should_return_false_by_default () {
                Assert.IsFalse(_node.IsPurchased);
            }
        }

        public class PurchaseMethod : NodeSkillTest {
            [Test]
            public void It_should_mark_the_node_as_IsPurchased_to_true () {
                _node.Purchase();
                
                Assert.IsTrue(_node.IsPurchased);
            }
        }

        public class RefundMethod : NodeSkillTest {
            [Test]
            public void It_should_mark_the_node_as_IsPurchased_to_false () {
                _node.Purchase();
                _node.Refund();
                
                Assert.IsFalse(_node.IsPurchased);
            }
        }        
    }
}
