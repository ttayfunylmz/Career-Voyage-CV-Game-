using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using ArcadeVehicleController;
using System.Collections.Generic;

public class TutorialUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vehicle vehicle;

    [Header("Visuals")]
    [SerializeField] private Image rOutlineImage;
    [SerializeField] private Image wImage;
    [SerializeField] private Image aImage;
    [SerializeField] private Image sImage;
    [SerializeField] private Image dImage;
    [SerializeField] private Image hImage;

    [Header("Sprites")]
    [SerializeField] private Sprite hClickSprite;
    [SerializeField] private Sprite hNonClickSprite;

    [Header("Settings")]
    [SerializeField] private float timerMax = 10f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float scaleDuration = 0.5f;

    private float timer;

    private Dictionary<KeyCode, Image> keyImageMap;

    private void Awake() 
    {
        keyImageMap = new Dictionary<KeyCode, Image>
        {
            {KeyCode.W, wImage},
            {KeyCode.A, aImage},
            {KeyCode.D, dImage},
            {KeyCode.S, sImage}
        };
    }

    private void Start()
    {
        timer = timerMax;
        vehicle.OnVehicleFix += Vehicle_OnVehicleFix;
        StartCoroutine(UpdateTimer());

        AnimateImage(wImage);
        AnimateImage(aImage);
        AnimateImage(sImage);
        AnimateImage(dImage);
    }

    private void Update() 
    {
        HandleHornSpriteChanges();

        foreach(var key in keyImageMap)
        {
            if(Input.GetKeyDown(key.Key))
            {
                FadeOutImages(key.Value);
                break;
            }
        }
    }

    private void AnimateImage(Image image)
    {
        image.transform.DOScale(1.2f, scaleDuration).SetLoops(50, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void FadeOutImages(Image image)
    {
        if(image.gameObject.activeInHierarchy)
        {
            image.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                image.gameObject.SetActive(false);
            });
        }
    }

    private void HandleHornSpriteChanges()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            hImage.sprite = hClickSprite;
            AudioManager.Instance.Play(Consts.Sounds.HORN_SOUND);
        }

        if(Input.GetKeyUp(KeyCode.H))
        {
            hImage.sprite = hNonClickSprite;
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (timer < timerMax)
            {
                timer += Time.fixedDeltaTime;
                rOutlineImage.fillAmount = timer / timerMax;
                vehicle.SetIsFlipping(true);
                if(timer >= timerMax)
                {
                    vehicle.SetIsFlipping(false);
                }
            }
        }
    }

    private void Vehicle_OnVehicleFix()
    {
        timer = 0f;
        rOutlineImage.DOFillAmount(1f, timer);
    }

    private void OnDestroy() 
    {
        vehicle.OnVehicleFix -= Vehicle_OnVehicleFix;
    }
}
