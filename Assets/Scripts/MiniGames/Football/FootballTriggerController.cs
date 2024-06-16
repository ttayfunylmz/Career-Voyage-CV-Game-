using System;
using System.Collections;
using UnityEngine;

public class FootballTriggerController : MonoSingleton<FootballTriggerController>
{
    public event Action OnRightGoalEntered;
    public event Action OnLeftGoalEntered;
    public event Action OnOutsideTriggered;

    private WaitForSeconds delay = new(2f);
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (isTriggered) return;

        if (other.CompareTag(Consts.Tags.RIGHT_GOAL))
        {
            OnRightGoalEntered?.Invoke();
            isTriggered = true;
            StartCoroutine(ResetTriggerState());
        }
        else if (other.CompareTag(Consts.Tags.LEFT_GOAL))
        {
            OnLeftGoalEntered?.Invoke();
            isTriggered = true;
            StartCoroutine(ResetTriggerState());
        }
        else if(other.CompareTag(Consts.Tags.OUTSIDE_TRIGGER))
        {
            OnOutsideTriggered?.Invoke();
            isTriggered = true;
            StartCoroutine(ResetTriggerState());
        }
    }

    private IEnumerator ResetTriggerState()
    {
        yield return delay;
        isTriggered = false;
    }
}
