using UnityEngine;
using System;
using UnityEngine.UI;

public class BobbingObject : MonoBehaviour
{
    [SerializeField] float floatStrength = 0.2f;
    private float originalY;
    private float phaseOffset;
    private Transform objectTransform;

    private void Awake()
    {
        objectTransform = GetComponent<Transform>();
        originalY = objectTransform.position.y;
        phaseOffset = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
    }

    private void Update()
    {
        float newY = originalY + Mathf.Sin(Time.time + phaseOffset) * floatStrength;
        objectTransform.position = new Vector2(objectTransform.position.x, newY);
    }
}
