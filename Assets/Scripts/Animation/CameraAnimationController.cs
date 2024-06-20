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
    [SerializeField] private GameObject jeepGameObject;

    [Header("Settings")]
    [SerializeField] private float firstAnimationDuration = 0.75f;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Ease animationEase = Ease.OutBack;
    [SerializeField] private bool isInteracting;
    [SerializeField] private bool isInteractionEnding;

    private Vector3 targetRotationVector = new (0f, -90f, 0f);
    private Vector3 firstCameraPosition;
    private Vector3 firstCameraRotation;
    private Transform previousTargetTransformGlobal;
    private Rigidbody jeepRigidbody;

    private void Start() 
    {
        jeepRigidbody = jeepGameObject.GetComponent<Rigidbody>();
        this.enabled = false;
    }

    public void OnInteraction(Transform targetTransform, Transform previousTargetTransform)
    {
        if(isInteracting || isInteractionEnding) { return; }

        isInteracting = true;

        previousTargetTransformGlobal = previousTargetTransform;
        jeepRigidbody.isKinematic = true;
        thirdPersonCameraController.enabled = false;
        playerController.enabled = false;
        playerInteractionGameObject.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration);
        
        firstCameraPosition = mainCamera.transform.position;
        firstCameraRotation = mainCamera.transform.rotation.eulerAngles;

        AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND);

        mainCamera.transform.DOMove(previousTargetTransform.position, firstAnimationDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND2);
            mainCamera.transform.DOMove(targetTransform.position, animationDuration).SetEase(animationEase).OnComplete(() =>
            {
                isInteracting = false;
            });
        });

        mainCamera.transform.DORotate(targetRotationVector, animationDuration).SetEase(animationEase);
    }

    public void OnInteractionEnd()
    {
        if(isInteracting || isInteractionEnding) { return; }

        isInteractionEnding = true;

        AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND);

        mainCamera.transform.DOMove(previousTargetTransformGlobal.position, firstAnimationDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            AudioManager.Instance.Play(Consts.Sounds.CAMERA_TRANSITION_SOUND2);
            mainCamera.transform.DOMove(firstCameraPosition, animationDuration).SetEase(animationEase).OnComplete(() =>
            {
                thirdPersonCameraController.enabled = true;
                playerController.enabled = true;
                jeepRigidbody.isKinematic = false;
                playerInteractionGameObject.GetComponent<CanvasGroup>().DOFade(1f, fadeDuration);
                isInteractionEnding = false;
                this.enabled = false;
            });
        });

        mainCamera.transform.DORotate(firstCameraRotation, animationDuration).SetEase(animationEase);
    }

    public bool IsInteracting()
    {
        return isInteracting;
    }

    public bool IsInteractionEnding()
    {
        return isInteractionEnding;
    }

}
