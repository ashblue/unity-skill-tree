using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes.Editors {
    public class NodeGroupTest {
        private NodeGroup _group;
        private INode _child;

        [SetUp]
        public void BeforeEach () {
            _group = new NodeGroup();
            _child = Substitute.For<INode>();
        }
        
        public class AddChildMethod : NodeGroupTest {
            [Test]
            public void It_should_add_a_child () {
                _group.AddChild(_child);
                
                Assert.AreEqual(1, _group.Children.Count);
            }
        }

        public class EnableMethod : NodeGroupTest {
            [Test]
            public void It_should_call_Enable_on_children () {
                _group.AddChild(_child);

                _group.Enable(SkillType.Ability, true);
                
                _child.Received(1).Enable(SkillType.Ability, true);
            }
        }
        
        public class DisableMethod : NodeGroupTest {
            [Test]
            public void It_should_call_Disable_on_children () {
                _group.AddChild(_child);

                _group.Disable(SkillType.Ability);
                
                _child.Received(1).Disable(SkillType.Ability);
            }
        }

        public class ParentPurchased : NodeGroupTest {
            [Test]
            public void It_should_call_ParentPurchased_on_children () {
                _group.AddChild(_child);
                
                _group.ParentPurchased();
                
                _child.Received(1).ParentPurchased();
            }
        }

        public class BindChildrenToExitMethod : NodeGroupTest {
            [Test]
            public void It_should_bind_an_empty_childs_Purchase_event_to_trigger_exit_childs_ParentPurchased () {
                var exit = Substitute.For<INode>();
                var child = new NodeSkill();
                
                _group.AddChild(child);
                _group.GroupExit.Add(exit);
                _group.BindChildrenToExit();
                child.Purchase();
                
                exit.Received(1).ParentPurchased();
            }
            
            [Test]
            public void It_should_bind_an_empty_childs_Refund_event_to_trigger_exit_childs_ParentRefund () {
                var exit = Substitute.For<INode>();
                var child = new NodeSkill();
                
                _group.AddChild(child);
                _group.GroupExit.Add(exit);
                _group.BindChildrenToExit();
                child.Refund();
                
                exit.Received(1).ParentRefund();
            }
            
            [Test]
            public void It_should_bind_an_empty_childs_ParentRefund_event_to_trigger_exit_childs_ParentRefund () {
                var exit = Substitute.For<INode>();
                var child = new NodeSkill();
                
                _group.AddChild(child);
                _group.GroupExit.Add(exit);
                _group.BindChildrenToExit();
                child.ParentRefund();
                
                exit.Received(1).ParentRefund();
            }

            [Test]
            public void It_should_not_trigger_ParentRefund_on_exit_if_one_of_two_parents_are_active () {
                var exit = Substitute.For<INode>();
                var child = new NodeSkill();
                var childAlt = new NodeSkill();
                childAlt.Purchase();

                _group.AddChild(child);
                _group.AddChild(childAlt);
                _group.GroupExit.Add(exit);
                _group.BindChildrenToExit();
                child.Refund();
                
                exit.Received(0).ParentRefund();
            }
        }
    }
}