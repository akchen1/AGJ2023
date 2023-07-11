using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(InventoryInteraction))]
public class InventoryInteractionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InventoryInteraction inventoryInteraction = (InventoryInteraction)target;

        if (inventoryInteraction.RequiredSceneInteraction)
        {
            GUIContent scenes = new GUIContent("Scene");
            int index = Constants.SceneNamesArray.ToList().IndexOf(inventoryInteraction.selectedSceneName);
            index = index < 0 ? 0 : index;
            index = EditorGUILayout.Popup(scenes, index, Constants.SceneNamesArray);
            inventoryInteraction.selectedSceneName = Constants.SceneNamesArray[index];

            if (inventoryInteraction.selectedSceneName != Constants.SceneNames.SearchScene7MainStreet) return;
            GUIContent subScenes = new GUIContent("Subscene");
            inventoryInteraction.selectedSubsceneScene = (Constants.Scene7SubScenes)EditorGUILayout.EnumPopup(subScenes, inventoryInteraction.selectedSubsceneScene);
        }

    }
}