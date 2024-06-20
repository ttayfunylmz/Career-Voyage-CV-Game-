using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsUI : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;
    [SerializeField] private AudioManager audioManager;

    [Header("Buttons")]
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Button sfxToggleButton;

    [Header("Sprites")]
    [SerializeField] private Sprite musicOnIcon;
    [SerializeField] private Sprite musicOffIcon;
    [SerializeField] private Sprite sfxOnIcon;
    [SerializeField] private Sprite sfxOffIcon;

    private bool isMusicOn = true;
    private bool areSfxOn = true;

    private void Start()
    {
        musicToggleButton.onClick.AddListener(ToggleMusic);
        sfxToggleButton.onClick.AddListener(ToggleSfx);

        UpdateMusicButtonIcon();
        UpdateSfxButtonIcon();
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
