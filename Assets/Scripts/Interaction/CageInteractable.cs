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
    private bool isInteracted;

    public void Interact()
    {
        if(isInteracted) { return; }
        
        AnimateFence();
        StartCoroutine(InteractSpriteUpdater.Instance.OnKeyReleasedCoroutine());
    }

    private void AnimateFence()
    {
        AudioManager.Instance.Play(Consts.Sounds.FENCE_OPENING_SOUND);
        fenceParent.DORotate(targetRotation, animationDuration);
        isInteracted = true;
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
