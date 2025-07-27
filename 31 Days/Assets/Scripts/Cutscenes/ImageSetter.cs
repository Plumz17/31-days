using UnityEngine;
using UnityEngine.UI;

public class ImageSetter : MonoBehaviour
{
    public Image targetImage;       
    public Sprite newSprite;    

    public void SetImage()
    {
        targetImage.sprite = newSprite;
    }
}
