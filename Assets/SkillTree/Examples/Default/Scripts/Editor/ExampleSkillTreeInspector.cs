using UnityEditor;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Examples.Editors {
    [CustomEditor(typeof(ExampleSkillTree))]
    public class ExampleSkillTreeInspector : Editor {
        private ExampleSkillTree.SaveTree _save;
        
        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;

            var skillTree = target as ExampleSkillTree;
            
            if (GUILayout.Button("Save")) {
                _save = skillTree.Save();
            }

            if (_save != null && GUILayout.Button("Load")) {
                skillTree.Load(_save);
            }
        }
    }
}