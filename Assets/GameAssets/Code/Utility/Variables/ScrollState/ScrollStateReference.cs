using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScrollStateReference
{
    public bool UseConstant = true;
    public ScrollState ConstantValue;
    public ScrollStateVariable Variable;

    public ScrollStateReference()
    { }

    public ScrollStateReference(ScrollState value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public ScrollState Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator ScrollState(ScrollStateReference reference)
    {
        return reference.Value;
    }
}
