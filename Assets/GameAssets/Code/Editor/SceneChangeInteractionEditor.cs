using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneChangeInteraction))]
public class SceneChangeInteractionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneChangeInteraction sceneChangeInteraction = (SceneChangeInteraction)target;


        GUIContent scenes = new GUIContent("Scene");
        int index = Constants.SceneNamesArray.ToList().IndexOf(sceneChangeInteraction.selectedSceneName);
        index = index < 0 ? 0 : index;
        index = EditorGUILayout.Popup(scenes, index, Constants.SceneNamesArray);
        sceneChangeInteraction.selectedSceneName = Constants.SceneNamesArray[index];
    }
}
