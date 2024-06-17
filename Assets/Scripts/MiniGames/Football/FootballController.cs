using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FootballController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform footballTransform;
    [SerializeField] private Rigidbody footballRigidbody;
    [SerializeField] private TMP_Text leftScoreText;
    [SerializeField] private TMP_Text rightScoreText;
    [SerializeField] private ParticleSystem leftConfettiParticles;
    [SerializeField] private ParticleSystem rightConfettiParticles;

    [Header("Settings")]
    [SerializeField] private float waitingSeconds = 2f;

    private int leftScore = 0;
    private int rightScore = 0;
    private Vector3 startPosition;

    private void Awake() 
    {
        startPosition = footballTransform.position;    
    }

    private void Start() 
    {
        FootballTriggerController.Instance.OnLeftGoalEntered += OnLeftGoalEntered;
        FootballTriggerController.Instance.OnRightGoalEntered += OnRightGoalEntered;
        FootballTriggerController.Instance.OnOutsideTriggered += OnOutsideTriggered;
    }

    private void OnOutsideTriggered()
    {
        StartCoroutine(ResetFootball());
    }

    private void OnRightGoalEntered()
    {
        IncreaseScore(ref rightScore, leftScoreText);
        PlayConfetti(rightConfettiParticles);
        StartCoroutine(ResetFootball());
    }

    private void OnLeftGoalEntered()
    {
        IncreaseScore(ref leftScore, rightScoreText);
        PlayConfetti(leftConfettiParticles);
        StartCoroutine(ResetFootball());
    }

    private void IncreaseScore(ref int score, TMP_Text text)
    {
        score++;
        text.text = score.ToString();
    }

    private void PlayConfetti(ParticleSystem confettiParticles)
    {
        confettiParticles.Play();
        AudioManager.Instance.Play(Consts.Sounds.CONFETTI_SOUND);
    }

    private IEnumerator ResetFootball()
    {
        footballRigidbody.isKinematic = true;
        yield return new WaitForSeconds(waitingSeconds);

        AudioManager.Instance.Play(Consts.Sounds.WHISTLE_SOUND);
        footballTransform.position = startPosition;
        yield return null;

        footballRigidbody.isKinematic = false;
    }
}
