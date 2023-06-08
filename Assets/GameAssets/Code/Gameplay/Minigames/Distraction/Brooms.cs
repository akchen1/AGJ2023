using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brooms : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "Oil")
        {
            Debug.Log("Finish Game");
        }
    }
}
