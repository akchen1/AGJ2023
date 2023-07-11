using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueClip))]
[CanEditMultipleObjects]
public class DialogueClipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueClip dialogueClip = (DialogueClip)target;
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.TextArea(dialogueClip.Dialogue.Character.CharacterName);
        GUILayout.TextArea(dialogueClip.Dialogue.Text);
        EditorGUI.EndDisabledGroup();
    }
}
