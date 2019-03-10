using UnityEngine;
using XNodeEditor;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes.Editor {
    [CustomNodeEditor(typeof(SkillNode))]
    public class SkillNodeEditor : NodeEditor {
        public override void OnBodyGUI () {
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("enter"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_displayName"));

            var image = serializedObject.FindProperty("_graphic").objectReferenceValue as Sprite;
            if (image != null) {
                GUILayout.Box(image.texture, GUILayout.Width(50), GUILayout.Height(50));                
            }
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_purchased"));

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("children"));
        }
    }
}