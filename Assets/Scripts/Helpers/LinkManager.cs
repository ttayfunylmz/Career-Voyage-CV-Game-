using System;
using UnityEngine;
using UnityEngine.UI;

public class LinkManager : MonoBehaviour
{
    [SerializeField] private string link;
    private Button _button;

    private void Awake() 
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OpenLink);    
    }

    private void OpenLink()
    {
        AudioManager.Instance.Play(Consts.Sounds.BUTTON_CLICK_SOUND);
        Application.OpenURL(link);
    }
}
