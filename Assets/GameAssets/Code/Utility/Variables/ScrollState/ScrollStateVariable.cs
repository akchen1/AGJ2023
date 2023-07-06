using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/ScrollState")]
public class ScrollStateVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public ScrollState Value;

    public void SetValue(ScrollState value)
    {
        Value = value;
    }

    public void SetValue(ScrollStateVariable value)
    {
        Value = value.Value;
    }
}
