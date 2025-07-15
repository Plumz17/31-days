using UnityEngine;
using System;
using UnityEngine.UI;

public class BobbingUI : MonoBehaviour
{
    [SerializeField] float floatStrength = 0.2f;
    private float originalY;
    private float phaseOffset;

    private void Awake() 
    {
        originalY = transform.position.y;
        phaseOffset = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
    }

    private void Update() //Handle Bobbing effect
    {
        float newY = originalY + Mathf.Sin(Time.time + phaseOffset) * floatStrength;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
