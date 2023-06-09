using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeeBooks : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject panel;
    public void OnPointerClick(PointerEventData eventData)
    {
        panel.SetActive(true);
    }
}
