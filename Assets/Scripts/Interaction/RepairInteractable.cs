using System.Collections.Generic;
using UnityEngine;

public class RepairInteractable : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private string interactText;
    [SerializeField] private List<Transform> brickTransforms = new();
    [SerializeField] private List<Vector3> brickStartPositions = new();

    private void Awake() 
    {
        // for(int i = 0; i < brickTransforms.Count; ++i)
        // {
        //     brickTransforms[i].position = brickStartPositions[i];
        //     brickStartPositions.Add(brickTransforms[i].position);
        // }
    }

    public void Interact()
    {
        Debug.Log("Repair Interacted");
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
