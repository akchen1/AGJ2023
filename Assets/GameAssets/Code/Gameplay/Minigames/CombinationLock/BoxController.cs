using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxController : MonoBehaviour
{
    public Sprite OpenBoxSprite;

    public void OpenBox()
    {
        GetComponent<Image>().sprite = OpenBoxSprite;
    }
}
