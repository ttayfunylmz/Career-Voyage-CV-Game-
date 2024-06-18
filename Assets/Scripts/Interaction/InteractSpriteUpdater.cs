using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class InteractSpriteUpdater : MonoSingleton<InteractSpriteUpdater>
{
    [Header("References")]
    [SerializeField] private Sprite keyLongSprite;
    [SerializeField] private Sprite keyShortSprite;
    [SerializeField] private Image keyBackgroundImage;
    [SerializeField] private RectTransform eText;

    [Header("Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float waitingSeconds = 1f;

    public void OnKeyPressed()
    {
        if(CameraAnimationController.Instance.IsInteractionEnding()) { return; }
        
        keyBackgroundImage.DOColor(Color.red, animationDuration);
        keyBackgroundImage.sprite = keyShortSprite;
        eText.anchoredPosition = new Vector2(0, 22f);
    }

    public void OnKeyReleased()
    {
        keyBackgroundImage.DOColor(Color.white, animationDuration);
        keyBackgroundImage.sprite = keyLongSprite;
        eText.anchoredPosition = new Vector2(0, 37f);
    }

    public IEnumerator OnKeyReleasedCoroutine()
    {
        OnKeyPressed();
        yield return new WaitForSeconds(waitingSeconds);
        OnKeyReleased();
    }

}
