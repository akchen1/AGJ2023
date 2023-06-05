using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LockEvent
{
    public class GetCombination
    {
        public GetCombination(Action<string, int> processData)
        {
            ProcessData = processData;
        }
        public readonly Action<string,int> ProcessData;

    }
}
