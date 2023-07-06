using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

[CustomEditor(typeof(AnimatorParameterMarkerEmitter))]
public class AnimatorControllerParameterDrawer : Editor
{
    private SerializedProperty parameterValuesProperty;
    private GUIStyle addButtonStyle;
    private GUIStyle removeButtonStyle;

    private void OnEnable()
    {
        parameterValuesProperty = serializedObject.FindProperty("ParameterValues");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        CreateButtonStyles();

        //EditorGUILayout.PropertyField(parameterValuesProperty, true);

        if (parameterValuesProperty.isArray)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 15f);
            if (GUILayout.Button("+", addButtonStyle, GUILayout.Width(20f)))
            {
                parameterValuesProperty.InsertArrayElementAtIndex(parameterValuesProperty.arraySize);
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < parameterValuesProperty.arraySize; i++)
            {
                var parameterValueProperty = parameterValuesProperty.GetArrayElementAtIndex(i);
                var parameterTypeProperty = parameterValueProperty.FindPropertyRelative("ParameterType");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(parameterTypeProperty);
                if (GUILayout.Button("-", removeButtonStyle, GUILayout.Width(20f)))
                {
                    parameterValuesProperty.DeleteArrayElementAtIndex(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();

                var parameterType = (AnimatorControllerParameterType)parameterTypeProperty.intValue;

                // Show/hide properties based on the selected ParameterType
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(parameterValueProperty.FindPropertyRelative("ParameterName"));

                switch (parameterType)
                {
                    case AnimatorControllerParameterType.Float:
                        EditorGUILayout.PropertyField(parameterValueProperty.FindPropertyRelative("ParameterFloatValue"));
                        break;
                    case AnimatorControllerParameterType.Int:
                        EditorGUILayout.PropertyField(parameterValueProperty.FindPropertyRelative("ParameterIntValue"));
                        break;
                    case AnimatorControllerParameterType.Bool:
                        EditorGUILayout.PropertyField(parameterValueProperty.FindPropertyRelative("ParameterBoolValue"));
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        EditorGUILayout.PropertyField(parameterValueProperty.FindPropertyRelative("ParameterBoolValue"));
                        break;
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void CreateButtonStyles()
    {
        addButtonStyle = new GUIStyle(GUI.skin.button);
        addButtonStyle.normal.textColor = Color.green;

        removeButtonStyle = new GUIStyle(GUI.skin.button);
        removeButtonStyle.normal.textColor = Color.red;
    }
}
