using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BooleanReference
{
    public bool UseConstant = true;
    public bool ConstantValue;
    public BooleanVariable Variable;

    public BooleanReference()
    { }

    public BooleanReference(bool value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public bool Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator bool(BooleanReference reference)
    {
        return reference.Value;
    }
}
