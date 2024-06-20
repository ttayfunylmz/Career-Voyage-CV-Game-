using System.Collections.Generic;
using ArcadeVehicleController;
using UnityEngine;

public class JeepInteract : MonoBehaviour
{
    private Vehicle vehicle;

    private void Awake() 
    {
        vehicle = GetComponent<Vehicle>();    
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            IInteractable interactable = GetInteractableObject();
            
            if(interactable != null && !vehicle.GetIsFlippingCompleted())
            {
                interactable.Interact();
                AudioManager.Instance.Play(Consts.Sounds.KEYBOARD_CLICK_SOUND);
            }
        }
    }

    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach(Collider collider in colliderArray)
        {
            if(collider.TryGetComponent(out IInteractable interactable))
            {
                interactableList.Add(interactable);
            }
        }
        
        IInteractable closestInteractable = null;
        foreach(IInteractable interactable in interactableList)
        {
            if(closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if(Vector3.Distance(transform.position, interactable.GetTransform().position) < Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    //This one is closer
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }
}
