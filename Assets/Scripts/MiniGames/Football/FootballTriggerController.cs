using System;
using UnityEngine;

public class FootballTriggerController : MonoSingleton<FootballTriggerController>
{
    public event Action OnRightGoalEntered;
    public event Action OnLeftGoalEntered;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (isTriggered) return;

        if (other.CompareTag(Consts.Tags.RIGHT_GOAL))
        {
            OnRightGoalEntered?.Invoke();
            isTriggered = true;
        }
        else if (other.CompareTag(Consts.Tags.LEFT_GOAL))
        {
            OnLeftGoalEntered?.Invoke();
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Consts.Tags.RIGHT_GOAL) || other.CompareTag(Consts.Tags.LEFT_GOAL))
        {
            isTriggered = false;
        }
    }
}
