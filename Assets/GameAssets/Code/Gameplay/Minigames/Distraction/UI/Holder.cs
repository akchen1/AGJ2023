using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Holder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColor;// Set this to the holder's original color
    public Color hoverColor = Color.green; // Set this to the color you want when hovering
    public bool isHovering = false;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    private void Update() 
    {
        if (isHovering && DragAndDrop.isDragging)
        {
            image.color = hoverColor;
        }
        else
        {
            image.color = defaultColor;
        }
    }
}