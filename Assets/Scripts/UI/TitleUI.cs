using DG.Tweening;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform textsToAnimateTransform;

    [Header("Settings")]
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float waitingDuration = 3f;

    private Vector2 startPosition;

    private void Awake() 
    {
        startPosition = textsToAnimateTransform.anchoredPosition; 
    }

    private void Start()
    {
        AnimateTexts();
    }

    private void AnimateTexts()
    {
        AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND);
        textsToAnimateTransform.DOAnchorPosY(0f, moveDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Invoke(nameof(ResetTexts), waitingDuration);
        });
    }

    private void ResetTexts()
    {
        AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND2);
        textsToAnimateTransform.DOAnchorPosY(startPosition.y, moveDuration).SetEase(Ease.OutQuart);
    }
}
