using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

[CustomEditor(typeof(SceneChangeMarkerEmitter))]
public class SceneChangeMarkerEmitterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneChangeMarkerEmitter sceneChangeMarkerEmitter = (SceneChangeMarkerEmitter)target;

        GUIContent scenes = new GUIContent("Scene");

        int index = Constants.SceneNamesArray.ToList().IndexOf(sceneChangeMarkerEmitter.selectedSceneName);
        index = index < 0 ? 0 : index;
        index = EditorGUILayout.Popup(scenes, index, Constants.SceneNamesArray);
        sceneChangeMarkerEmitter.selectedSceneName = Constants.SceneNamesArray[index];

    }
}
