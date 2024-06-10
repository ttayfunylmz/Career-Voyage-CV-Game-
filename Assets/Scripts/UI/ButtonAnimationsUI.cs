using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimationsUI : MonoBehaviour
{
    [SerializeField] private string link;
    [SerializeField] private float waitDuration;
    [SerializeField] private float shakeDuration;

    private Button _button;
    private RectTransform _rectTransform;

    private void Awake() 
    {
        _button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();

        _button.onClick.AddListener(OnButtonClick);    
    }

    private void Start() 
    {
        // StartCoroutine(AnimateButton());  
    }

    private void OnButtonClick()
    {
        OpenLink(link);
    }

    private void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    // private IEnumerator AnimateButton()
    // {
        
    // }
}
