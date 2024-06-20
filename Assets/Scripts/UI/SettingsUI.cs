using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;
    [SerializeField] private AudioManager audioManager;

    [Header("References")]
    [SerializeField] private Button scrollButton;
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Button sfxToggleButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private RectTransform arrowTransform;

    [Header("Sprites")]
    [SerializeField] private Sprite musicOnIcon;
    [SerializeField] private Sprite musicOffIcon;
    [SerializeField] private Sprite sfxOnIcon;
    [SerializeField] private Sprite sfxOffIcon;

    [Header("Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float rotateDuration = 0.5f;
    [SerializeField] private bool isScrollActive;

    private RectTransform scrollButtonRectTransform;

    private bool isMusicOn = true;
    private bool areSfxOn = true;

    private void Awake() 
    {
        scrollButtonRectTransform = scrollButton.GetComponent<RectTransform>();

        scrollButton.onClick.AddListener(OnScrollButtonClick);
        musicToggleButton.onClick.AddListener(ToggleMusic);
        sfxToggleButton.onClick.AddListener(ToggleSfx);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void Start()
    {
        UpdateMusicButtonIcon();
        UpdateSfxButtonIcon();
    }

    private void OnScrollButtonClick()
    {
        isScrollActive = !isScrollActive;
        if (isScrollActive)
        {
            AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND2);
            scrollButtonRectTransform.DOAnchorPosY(430f, animationDuration).SetEase(Ease.OutQuart);
            arrowTransform.DORotate(Vector3.zero, rotateDuration).SetEase(Ease.OutQuart);
        }
        else
        {
            AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND);
            scrollButtonRectTransform.DOAnchorPosY(0f, animationDuration).SetEase(Ease.OutQuart);
            arrowTransform.DORotate(new Vector3(0f, 0f, 180f), rotateDuration).SetEase(Ease.OutQuart);
        }
    }

    private void ToggleMusic()
    {
        audioManager.Play(Consts.Sounds.BUTTON_CLICK_SOUND);

        isMusicOn = !isMusicOn;
        if (isMusicOn)
        {
            musicSource.UnPause();
        }
        else
        {
            musicSource.Pause();
        }

        UpdateMusicButtonIcon();
    }

    private void ToggleSfx()
    {
        audioManager.Play(Consts.Sounds.BUTTON_CLICK_SOUND);

        areSfxOn = !areSfxOn;
        foreach (var sfxSource in sfxSources)
        {
            if (areSfxOn)
            {
                sfxSource.UnPause();
                sfxSource.mute = false;
                audioManager.UnmuteAllSoundEffects();
            }
            else
            {
                sfxSource.Pause();
                sfxSource.mute = true;
                audioManager.MuteAllSoundEffects();
            }
        }

        UpdateSfxButtonIcon();
    }

    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(Consts.GameLogic.GAME_SCENE);
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }

    private void UpdateMusicButtonIcon()
    {
        if (isMusicOn)
        {
            musicToggleButton.image.sprite = musicOnIcon;
        }
        else
        {
            musicToggleButton.image.sprite = musicOffIcon;
        }
    }

    private void UpdateSfxButtonIcon()
    {
        if (areSfxOn)
        {
            sfxToggleButton.image.sprite = sfxOnIcon;
        }
        else
        {
            sfxToggleButton.image.sprite = sfxOffIcon;
        }
    }
}
