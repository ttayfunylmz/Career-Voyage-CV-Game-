using ArcadeVehicleController;
using UnityEngine;
using DG.Tweening;

public class CameraAnimationController : MonoSingleton<CameraAnimationController>
{
    [Header("References")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private ThirdPersonCameraController thirdPersonCameraController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject playerInteractionGameObject;

    [Header("Settings")]
    [SerializeField] private float firstAnimationDuration = 0.75f;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Ease animationEase = Ease.OutBack;

    private Vector3 targetRotationVector = new (0f, -90f, 0f);
    private Vector3 firstCameraPosition;
    private Vector3 firstCameraRotation;
    private bool isInteracting;
    private Transform previousTargetTransformGlobal;

    public void OnInteraction(Transform targetTransform, Transform previousTargetTransform)
    {
        isInteracting = true;

        previousTargetTransformGlobal = previousTargetTransform;

        thirdPersonCameraController.enabled = false;
        playerController.enabled = false;
        playerInteractionGameObject.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration);
        
        firstCameraPosition = mainCamera.transform.position;
        firstCameraRotation = mainCamera.transform.rotation.eulerAngles;

        AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND);

        mainCamera.transform.DOMove(previousTargetTransform.position, firstAnimationDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            mainCamera.transform.DOMove(targetTransform.position, animationDuration).SetEase(animationEase);
        });

        mainCamera.transform.DORotate(targetRotationVector, animationDuration).SetEase(animationEase);
    }

    public void OnInteractionEnd()
    {
        mainCamera.transform.DOMove(previousTargetTransformGlobal.position, firstAnimationDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            mainCamera.transform.DOMove(firstCameraPosition, animationDuration).SetEase(animationEase).OnComplete(() =>
            {
                thirdPersonCameraController.enabled = true;
                playerController.enabled = true;
                playerInteractionGameObject.GetComponent<CanvasGroup>().DOFade(1f, fadeDuration);
            });

            mainCamera.transform.DORotate(firstCameraRotation, animationDuration).SetEase(animationEase);
        });

        isInteracting = false;
    }

    public bool IsInteracting()
    {
        return isInteracting;
    }

}
