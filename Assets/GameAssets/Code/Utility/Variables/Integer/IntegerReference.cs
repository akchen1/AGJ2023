using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntegerReference
{
    public bool UseConstant = true;
    public int ConstantValue;
    public IntegerVariable Variable;

    public IntegerReference()
    { }

    public IntegerReference(int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public int Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator int(IntegerReference reference)
    {
        return reference.Value;
    }
}
