using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Boolean")]
public class BooleanVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public bool Value;

    public void SetValue(bool value)
    {
        Value = value;
    }

    public void SetValue(BooleanVariable value)
    {
        Value = value.Value;
    }
}
