using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Holder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColor = Color.white; // Set this to the holder's original color
    public Color hoverColor = Color.green; // Set this to the color you want when hovering

    public GameObject child;

    private Image image;
    private bool isHovering = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start() {
        transform.position = child.transform.position + new Vector3(0, 20, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UpdateColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UpdateColor();
    }

    private void UpdateColor()
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