using ArcadeVehicleController;
using UnityEngine;

public class FrameInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform previousTargetTransform;

    [Header("Settings")]
    [SerializeField] private string interactText;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape) && CameraAnimationController.Instance.IsInteracting())
        {
            CameraAnimationController.Instance.OnInteractionEnd();
            InteractSpriteUpdater.Instance.OnKeyReleased();
        }
    }

    public void Interact()
    {
        CameraAnimationController.Instance.OnInteraction(targetTransform, previousTargetTransform);
        InteractSpriteUpdater.Instance.OnKeyPressed();
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
