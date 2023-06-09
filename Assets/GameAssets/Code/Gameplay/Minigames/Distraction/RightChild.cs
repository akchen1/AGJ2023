using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightChild : MonoBehaviour
{
    [SerializeField] private string childName = null;
    public bool Check()
    {
        if(childName != null){
            if(transform.GetChild(0).name == childName)
                return true;
        }
        return false;
    }
}
