using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;

[CustomEditor(typeof(SceneChangeMarkerEmitter))]
public class SceneChangeMarkerEmitterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneChangeMarkerEmitter sceneChangeMarkerEmitter = (SceneChangeMarkerEmitter)target;

        GUIContent scenes = new GUIContent("Scene");
        sceneChangeMarkerEmitter.selectedSceneIndex = EditorGUILayout.Popup(scenes, sceneChangeMarkerEmitter.selectedSceneIndex, Constants.SceneNamesArray);

    }
}
