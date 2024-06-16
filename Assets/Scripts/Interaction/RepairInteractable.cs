using System.Collections.Generic;
using UnityEngine;

public class RepairInteractable : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private string interactText;
    [SerializeField] private List<Transform> brickTransforms = new List<Transform>();

    private List<Vector3> initialPositions = new();
    private List<Quaternion> initialRotations = new();

    private void Awake()
    {
        foreach (Transform brickTransform in brickTransforms)
        {
            initialPositions.Add(brickTransform.position);
            initialRotations.Add(brickTransform.rotation);
        }
    }

    public void Interact()
    {
        StartCoroutine(InteractSpriteUpdater.Instance.OnKeyReleasedCoroutine());
        RepairBricks();
    }

    private void RepairBricks()
    {
        for (int i = 0; i < brickTransforms.Count; i++)
        {
            brickTransforms[i].SetPositionAndRotation(initialPositions[i], initialRotations[i]);
        }
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
