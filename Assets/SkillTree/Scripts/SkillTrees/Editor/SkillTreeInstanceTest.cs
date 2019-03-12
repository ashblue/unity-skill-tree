using System.Collections.Generic;
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

            public class AssigningProperties : SkillTreeInstanceTest {
                [Test]
                public void It_should_assign_purchased_state_on_skills () {
                    _child.IsPurchased.Returns(true);
                
                    _skillTree.Setup(_data);
                
                    Assert.IsTrue(_skillTree.Root.Children[0].IsPurchased);
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