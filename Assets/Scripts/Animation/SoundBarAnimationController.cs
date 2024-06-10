using UnityEngine;
using DG.Tweening;

public class SoundBarAnimationController : MonoBehaviour
{
    [SerializeField] private float scaleValue = 1.2f;
    [SerializeField] private float animationDuration = 0.1f;

    private void Start() 
    {
        AnimateSoundBar();    
    }

    private void AnimateSoundBar()
    {
        transform.DOScale(scaleValue, animationDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
