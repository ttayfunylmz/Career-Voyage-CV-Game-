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

    [Header("Settings")]
    [SerializeField] private int leftScore = 0;
    [SerializeField] private int rightScore = 0;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float waitingSeconds = 2f;

    private void Awake() 
    {
        startPosition = footballTransform.position;    
    }

    private void Start() 
    {
        FootballTriggerController.Instance.OnLeftGoalEntered += OnLeftGoalEntered;
        FootballTriggerController.Instance.OnRightGoalEntered += OnRightGoalEntered;
    }

    private void OnRightGoalEntered()
    {
        IncreaseScore(ref rightScore, leftScoreText);
        StartCoroutine(ResetFootball());
    }

    private void OnLeftGoalEntered()
    {
        IncreaseScore(ref leftScore, rightScoreText);
        StartCoroutine(ResetFootball());
    }

    private void IncreaseScore(ref int score, TMP_Text text)
    {
        score++;
        text.text = score.ToString();
    }

    private IEnumerator ResetFootball()
    {
        yield return new WaitForSeconds(waitingSeconds);
        footballRigidbody.isKinematic = true;
        footballTransform.position = startPosition;
        yield return null;
        footballRigidbody.isKinematic = false;
    }
}
