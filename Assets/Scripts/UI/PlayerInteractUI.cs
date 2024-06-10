using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerInteractUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject containerObject;
    [SerializeField] private JeepInteract jeepInteract;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private RectTransform interactParent;

    [Header("Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease animationEase;

    private void Update() 
    {
        if(jeepInteract.GetInteractableObject() != null)
        {
            Show(jeepInteract.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    private void Show(IInteractable interactable)
    {
        interactText.text = interactable.GetInteractText();
        interactParent.DOAnchorPos(Vector2.zero, animationDuration).SetEase(animationEase);
    }

    private void Hide()
    {
        Vector2 initialPosition = new Vector2(0f, -500f);
        interactParent.DOAnchorPos(initialPosition, animationDuration).SetEase(animationEase);
    }
}
