using UnityEngine;

public class AdjustPositions : MonoBehaviour
{
    [SerializeField] private RectTransform holderObject; // Assign your UI GameObject's RectTransform here in the Inspector
    [SerializeField] private int offSet = 10;

    private void Start()
    {
        // Get the RectTransform of the Holder
        RectTransform holderRectTransform = GetComponent<RectTransform>();

        // Get the height of the UI GameObject
        float height = holderObject.sizeDelta.y;

        if(!(transform.GetChild(0).name == "Empty"))
        {
            // Move the holder up by the height of the UI GameObject
            holderRectTransform.anchoredPosition += new Vector2(0, height + offSet);

            // Move the UI GameObject down by its own height
            holderObject.anchoredPosition -= new Vector2(0, height + offSet);
        }
    }
    
}
