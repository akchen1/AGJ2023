using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaThreshold : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float threshold = 0.5f;
    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
    }
}
