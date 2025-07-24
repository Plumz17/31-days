using UnityEngine;

public class PauseBlink : MonoBehaviour
{
    public Animator anim;
    public float minBlinkInterval = 3f;
    public float maxBlinkInterval = 5f;

    private float blinkTimer;
    private float nextBlinkTime;

    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        SetNextBlinkTime();
    }

    void Update()
    {
        blinkTimer += Time.unscaledDeltaTime;

        if (blinkTimer >= nextBlinkTime)
        {
            anim.SetTrigger("blink");
            SetNextBlinkTime();
        }
    }

    void SetNextBlinkTime()
    {
        blinkTimer = 0f;
        nextBlinkTime = Random.Range(minBlinkInterval, maxBlinkInterval);
    }
}
