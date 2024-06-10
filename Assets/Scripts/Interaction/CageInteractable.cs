using DG.Tweening;
using UnityEngine;

public class CageInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Transform fenceParent;

    [Header("Settings")]
    [SerializeField] private string interactText;
    [SerializeField] private float animationDuration = 2f;

    private Vector3 targetRotation = new Vector3(0f, 120f, 0f);

    public void Interact()
    {
        AnimateFence();
        StartCoroutine(InteractSpriteUpdater.Instance.OnKeyReleasedCoroutine());
    }

    private void AnimateFence()
    {
        fenceParent.DORotate(targetRotation, animationDuration);
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
