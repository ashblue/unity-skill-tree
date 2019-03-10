using System.Collections.Generic;
using CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Editors {
    public class SkillTreeInstanceTest {
        public class SetupMethod {
            private SkillTreeInstance _skillTree;
            private ISkillTreeData _data;
            private ISkillNode _child;
            
            [SetUp]
            public void BeforeEach () {
                _skillTree = new SkillTreeInstance();
                _data = Substitute.For<ISkillTreeData>();
                var root = Substitute.For<ISkillNode>();
                _child = Substitute.For<ISkillNode>();
                
                _data.Root.Returns(root);
                root.Children.Returns(new List<ISkillNode> {_child});
            }

            public class RecursiveDataCreation : SetupMethod {
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

            public class AssigningProperties : SetupMethod {
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
            }
        }
    }
}