using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSliderUI : MonoBehaviour
{
    [SerializeField] private RectTransform bar; // Bar pivot needs to be (0, 0)
    [SerializeField] private RectTransform slider;

    [SerializeField] private bool horizontal;
    [SerializeField] private bool vertical;

    [SerializeField] [Range(0f, 2f)] private float verticalBarSpeed;
    [SerializeField] [Range(0f, 2f)] private float horizontalBarSpeed;

    private float height;
    private float width;

    private Vector2 sliderDisplacement;

    void Start()
    {
        height = bar.rect.height / 2f;
        width = bar.rect.width / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        Oscillate();
    }

    public Vector2 GetSliderDisplacement()
    {
        Vector2 scaled01 = sliderDisplacement;
        scaled01.x = (sliderDisplacement.x + width) / (width * 2f);
        scaled01.y = (sliderDisplacement.y + height) / (height * 2f);
        return scaled01;
    }

    private void Oscillate()
    {
        Vector2 displacement = Vector2.zero;
        if (horizontal)
        {
            float tx = GetOscillationTime(horizontalBarSpeed);
            displacement.x += GetPositonDisplacement(width, tx);
        }

        if (vertical)
        {
            float ty = GetOscillationTime(verticalBarSpeed);
            displacement.y += GetPositonDisplacement(height, ty);
        }
        sliderDisplacement = displacement;
        slider.position = bar.rect.position + displacement;
    }

    private float GetOscillationTime(float speed)
    {
        return (Mathf.Sin(speed * Time.time * Mathf.PI) + 1f) / 2f;
    }

    private float GetPositonDisplacement(float bound, float t)
    {
        return Mathf.Lerp(-bound, bound, t);
    }
}
