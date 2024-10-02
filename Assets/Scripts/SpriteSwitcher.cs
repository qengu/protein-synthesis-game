using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{

    public bool isSwitched = false;

    [Header("References")]
    public Image image1;
    public Image image2;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchImage(Sprite sprite)
    {

        float aspectRatio = sprite.rect.width / sprite.rect.height;

        if (!isSwitched)
        {
            image2.sprite = sprite;
            image2.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
            animator.SetTrigger("SwitchFirst");
 
        }
        else
        {
            image1.sprite = sprite;
            image1.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
            animator.SetTrigger("SwitchSecond");
        }
        isSwitched = !isSwitched;
    }

    public void SetImage(Sprite sprite)
    {
        float aspectRatio = sprite.rect.width / sprite.rect.height;

        if (!isSwitched)
        {
            image1.sprite = sprite;
            image1.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
        }
        else
        {
            image2.sprite = sprite;
            image2.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
        }
    }

    public Sprite GetImage()
    {
        if (!isSwitched)
        {
            return image1.sprite;
        }
        else
        {
            return image2.sprite;
        }
    }

}
