using System.Collections.Generic;
using System.Runtime.InteropServices;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Editors {
    public class SkillTreeInstanceTest {
        private SkillTreeInstance _skillTree;
        private ISkillTreeData _data;
        private ISkillNode _child;
            
        [SetUp]
        public void BeforeEach () {
            _skillTree = new SkillTreeInstance();
            _data = Substitute.For<ISkillTreeData>();
            _data.GetCopy().Returns(_data);
            
            var root = Substitute.For<ISkillNode>();
            root.IsPurchased.Returns(true);
            _child = Substitute.For<ISkillNode>();
                
            _data.Root.Returns(root);
            root.Children.Returns(new List<ISkillNode> {_child});
        }
        
        public class SetupMethod {
            public class RecursiveDataCreation : SkillTreeInstanceTest {
                [Test]
                public void It_should_convert_a_root_node_with_its_child () {
                    _skillTree.Setup(_data);
                
                    Assert.AreEqual(1, _skillTree.Root.Children.Count);
                }

                [Test]
                public void It_should_convert_nested_skill_children_to_nodes () {
                    var nestedChild = Substitute.For<ISkillNode>();
                    _child.Children.Returns(new List<ISkillNode> {nestedChild});
                
                    _skillTree.Setup(_data);
                
                    Assert.AreEqual(1, _skillTree.Root.Children[0].Children.Count);
                }
            }

            public class GroupCreation : SkillTreeInstanceTest {
                private ISkillNode _group;
                
                [SetUp]
                public void SetupGroupCreation () {
                    _group = Substitute.For<ISkillNode>();
                    _group.Children.Returns(new List<ISkillNode>{Substitute.For<ISkillNode>()});
                    _group.IsGroup.Returns(true);
                    _data.Root.Children.Returns(new List<ISkillNode> { _group });
                    _group.GroupExit.Returns(new List<ISkillNode> { _child });
                }
                
                [Test]
                public void It_should_generate_a_group () {
                    _skillTree.Setup(_data);

                    Assert.IsTrue(_skillTree.Root.Children[0] is NodeGroup);
                }

                [Test]
                public void It_should_add_children_to_the_group () {
                    _group.Children.Returns(new List<ISkillNode> { _child });
                    
                    _skillTree.Setup(_data);

                    Assert.AreEqual(1, _skillTree.Root.Children[0].Children.Count);
                }

                [Test]
                public void It_should_set_Exit_skills () {
                    _skillTree.Setup(_data);

                    Assert.AreEqual(1, _skillTree.Root.Children[0].GroupExit.Count);
                }
                
                [Test]
                public void It_should_set_Exit_groups () {
                    _skillTree.Setup(_data);

                    Assert.AreEqual(1, _skillTree.Root.Children[0].GroupExit.Count);
                }
            }

            public class AssigningProperties : SkillTreeInstanceTest {
                [Test]
                public void It_should_assign_purchased_state_on_skills () {
                    _child.IsPurchased.Returns(true);
                
                    _skillTree.Setup(_data);
                
                    Assert.IsTrue(_skillTree.Root.Children[0].IsPurchased);
                }
                
                [Test]
                public void It_should_assign_purchased_state_on_group_children () {
                    var groupChild = Substitute.For<ISkillNode>();
                    groupChild.IsPurchased.Returns(true);
                    
                    var group = Substitute.For<ISkillNode>();
                    group.IsGroup.Returns(true);
                    group.IsPurchased.Returns(true);
                    group.Children.Returns(new List<ISkillNode> {groupChild});
                    _data.Root.Children.Returns(new List<ISkillNode> {group});

                    _skillTree.Setup(_data);
                
                    Assert.IsTrue(_skillTree.Root.Children[0].Children[0].IsPurchased);
                }
                
                [Test]
                public void It_should_clear_purchased_state_on_group_children_if_child_is_unpurchased () {
                    _child.IsPurchased.Returns(false);
                    
                    var groupChild = Substitute.For<ISkillNode>();
                    groupChild.IsPurchased.Returns(true);

                    var group = Substitute.For<ISkillNode>();
                    group.IsGroup.Returns(true);
                    group.IsPurchased.Returns(true);
                    group.Children.Returns(new List<ISkillNode> {groupChild});
                    _child.Children.Returns(new List<ISkillNode> {group});

                    _skillTree.Setup(_data);
                
                    Assert.IsFalse(_skillTree.Root.Children[0].Children[0].Children[0].IsPurchased);
                }
                
                [Test]
                public void It_should_not_assign_false_purchased_state_on_group_exit () {
                    _child.IsPurchased.Returns(false);
                    
                    var groupChild = Substitute.For<ISkillNode>();
                    var groupExit = Substitute.For<ISkillNode>();
                    groupExit.IsPurchased.Returns(true);

                    var group = Substitute.For<ISkillNode>();
                    group.IsGroup.Returns(true);
                    group.IsPurchased.Returns(true);
                    group.Children.Returns(new List<ISkillNode> {groupChild});
                    group.GroupExit.Returns(new List<ISkillNode> {groupExit});
                    _child.Children.Returns(new List<ISkillNode> {group});

                    _skillTree.Setup(_data);
                
                    Assert.IsFalse(_skillTree.Root.Children[0].Children[0].GroupExit[0].IsPurchased);
                }
                
                [Test]
                public void It_should_assign_enabled_state_on_group_exit () {
                    _child.IsPurchased.Returns(true);
                    
                    var groupChild = Substitute.For<ISkillNode>();
                    groupChild.IsPurchased.Returns(true);
                    
                    var groupExit = Substitute.For<ISkillNode>();
                    groupExit.IsPurchased.Returns(true);

                    var group = Substitute.For<ISkillNode>();
                    group.IsGroup.Returns(true);
                    group.IsPurchased.Returns(true);
                    group.Children.Returns(new List<ISkillNode> {groupChild});
                    group.GroupExit.Returns(new List<ISkillNode> {groupExit});
                    _child.Children.Returns(new List<ISkillNode> {group});

                    _skillTree.Setup(_data);
                
                    Assert.IsTrue(_skillTree.Root.Children[0].Children[0].GroupExit[0].IsPurchased);
                    Assert.IsTrue(_skillTree.Root.Children[0].Children[0].GroupExit[0].IsEnabled);
                }

                [Test]
                public void It_should_assign_Id_on_skills () {
                    _child.Id.Returns("id");
                
                    _skillTree.Setup(_data);
                
                    Assert.AreEqual("id", _skillTree.Root.Children[0].Id);
                }

                [Test]
                public void It_should_not_mark_a_child_as_purchased_if_the_parent_is_not () {
                    var child = Substitute.For<ISkillNode>();
                    child.IsPurchased.Returns(true);

                    _child.Children.Returns(new List<ISkillNode> {child});
                    _skillTree.Setup(_data);
                    
                    Assert.IsFalse(_skillTree.Root.Children[0].Children[0].IsPurchased);
                }
                
                [Test]
                public void It_should_mark_nested_nodes_as_purchased () {
                    var child = Substitute.For<ISkillNode>();
                    child.IsPurchased.Returns(true);
                    _child.IsPurchased.Returns(true);

                    _child.Children.Returns(new List<ISkillNode> {child});
                    _skillTree.Setup(_data);
                    
                    Assert.IsTrue(_skillTree.Root.Children[0].IsPurchased);
                    Assert.IsTrue(_skillTree.Root.Children[0].Children[0].IsPurchased);
                }
            }
        }

        public class SaveMethod : SkillTreeInstanceTest {
            [Test]
            public void It_should_return_a_list_of_Ids_with_purchase_state () {
                _child.Id.Returns("id");
                _child.IsPurchased.Returns(true);

                _skillTree.Setup(_data);
                var save = _skillTree.Save();
                
                Assert.AreEqual(save[0].id, "id");
                Assert.AreEqual(save[0].purchased, true);
            }
        }

        public class LoadMethod : SkillTreeInstanceTest {
            [Test]
            public void It_should_rebuild_the_skill_tree () {
                _skillTree.Setup(_data);
                _skillTree.Root.Children[0].Purchase();
                
                _skillTree.Load(_data, new List<SkillSave>());
                
                Assert.IsFalse(_skillTree.Root.Children[0].IsPurchased);
            }
            
            [Test]
            public void It_should_restore_skill_IsPurchased_by_id_to_true () {
                _child.Id.Returns("id");
                _skillTree.Setup(_data);
                _skillTree.Root.Children[0].Purchase();

                var save = _skillTree.Save();
                _skillTree.Load(_data, save);
                
                Assert.IsTrue(_skillTree.Root.Children[0].IsPurchased);
            }
            
            [Test]
            public void It_should_restore_skill_IsPurchased_by_id_to_false () {
                _child.Id.Returns("id");
                _skillTree.Setup(_data);

                var save = _skillTree.Save();
                _skillTree.Load(_data, save);
                
                Assert.IsFalse(_skillTree.Root.Children[0].IsPurchased);
            }
        }
    }
}