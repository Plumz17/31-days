using UnityEngine;

[ExecuteAlways]
public class CurvedVerticalLayout : MonoBehaviour
{
    public float verticalSpacing = 80f;
    public float curveIntensity = 0.05f; // Higher = more curve

    void Update()
    {
        // Get all active children
        var activeButtons = new System.Collections.Generic.List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
                activeButtons.Add(child);
        }

        int count = activeButtons.Count;
        if (count == 0) return;

        float centerIndex = (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            float y = -(i - centerIndex) * verticalSpacing;

            // Curve: X offset increases the further from center
            float offsetFromCenter = Mathf.Abs(i - centerIndex);
            float x = Mathf.Pow(offsetFromCenter, 2) * curveIntensity * verticalSpacing;

            activeButtons[i].localPosition = new Vector3(x, y, 0);
        }
    }
}