using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{

    private SpriteSwitcher switcher;
    private Animator animator;
    private RectTransform rect;

    private void Awake()
    {
        if (switcher == null) switcher = GetComponent<SpriteSwitcher>();
        if (animator == null) animator = GetComponent<Animator>();
        if (rect == null) rect = GetComponent<RectTransform>();
    }

    public void Setup(Sprite sprite)
    {
        switcher.SetImage(sprite);
    }

    public void Show(Vector2 coords)
    {
        animator.SetTrigger("Show");
        rect.localPosition = coords;
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void SwitchSprite(Sprite sprite)
    {
        if (switcher.GetImage() != sprite)
        {
            switcher.SwitchImage(sprite);
        }
    }

}
