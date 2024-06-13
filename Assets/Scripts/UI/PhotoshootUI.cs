using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PhotoshootUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhotoshootArea photoshootArea;
    [SerializeField] private Image photoshootImage;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float fadeInDuration = 0.01f;
    [SerializeField] private float fadeOutDuration = 0.2f;
    [SerializeField] private float waitingSeconds = 0.5f;

    private CanvasGroup canvasGroup;
    private bool isPhotoshootInProgress;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        photoshootArea.OnPhotoshoot += PhotoshootArea_OnPhotoshoot;
    }

    private void PhotoshootArea_OnPhotoshoot()
    {
        if (!isPhotoshootInProgress)
        {
            isPhotoshootInProgress = true;
                photoshootImage.DOFade(1f, fadeInDuration).OnComplete(() =>
                {
                    photoshootImage.DOFade(0f, fadeOutDuration);
                });
            canvasGroup.DOFade(1f, fadeDuration).OnComplete(() =>
            {
                Time.timeScale = 0f;
                StartCoroutine(ContinueTheGame());
            });
        }
    }

    private IEnumerator ContinueTheGame()
    {
        yield return new WaitForSecondsRealtime(waitingSeconds);
        Time.timeScale = 1f;
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            canvasGroup.alpha = 0f;
            isPhotoshootInProgress = false;
        });
    }
}
