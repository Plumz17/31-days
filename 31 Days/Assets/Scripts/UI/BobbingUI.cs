using UnityEngine;
using System;
using UnityEngine.UI;

public class BobbingUI : MonoBehaviour
{
    [SerializeField] float floatStrength = 0.2f;
    private float originalY;
    private float phaseOffset;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalY = rectTransform.anchoredPosition.y;
        phaseOffset = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
    }

    private void Update()
    {
        float newY = originalY + Mathf.Sin(Time.time + phaseOffset) * floatStrength;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}
