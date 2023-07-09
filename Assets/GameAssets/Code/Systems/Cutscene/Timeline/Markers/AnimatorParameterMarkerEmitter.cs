using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class AnimatorParameterMarkerEmitter : MarkerEmitter
{
    public AnimationParameter[] ParameterValues;
}
[System.Serializable]
public class AnimationParameter
{
    public string ParameterName;
    public AnimatorControllerParameterType ParameterType;
    public string ParameterStringValue;
    public bool ParameterBoolValue;
    public float ParameterFloatValue;
    public int ParameterIntValue;
}