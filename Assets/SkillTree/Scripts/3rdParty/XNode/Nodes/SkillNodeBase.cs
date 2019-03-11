using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    public abstract class SkillNodeBase : Node, ISkillNode {
        [Output(connectionType = ConnectionType.Multiple)]
        public Connection children;
        
        [SerializeField]
        private string _id;
        
        [Serializable]
        public class Connection {}
        
        public string Id => _id;

        public virtual Sprite Graphic => null;
        public virtual bool IsPurchased => false;
        public virtual string DisplayName => null;
        public virtual string Description => null;
        
        public List<ISkillNode> Children {
            get {
                var port = GetOutputPort("children");
                var list = new List<ISkillNode>();
                for (var i = 0; i < port.ConnectionCount; i++) {
                    list.Add(port.GetConnection(i).node as ISkillNode);
                }
    
                return list;
            }
        }

        
        protected override void Init () {
            GenerateId();
        }

        private void GenerateId () {
            if (_id == null) {
                _id = Guid.NewGuid().ToString();
            } else {
                var match = graph.nodes.Find(n => {
                    if (n == this) return false;
                    var skillNode = (ISkillNode)n;
                    return skillNode != null && skillNode.Id == _id;
                });

                if (match != null) {
                    _id = Guid.NewGuid().ToString();
                }
            }
        }
    }
}