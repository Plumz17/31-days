using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float popScale = 1.1f;
    public float popSpeed = 10f;
    public GameObject highlightArrow;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        if (highlightArrow != null)
            highlightArrow.SetActive(false);
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * popSpeed);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * popScale;
        if (highlightArrow != null)
            highlightArrow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        if (highlightArrow != null)
            highlightArrow.SetActive(false);
    }
}