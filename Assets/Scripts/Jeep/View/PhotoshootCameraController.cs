using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoshootCameraController : MonoBehaviour
{
    [SerializeField] private Transform vehicleTransform;

    private void LateUpdate() 
    {
        if(gameObject.activeInHierarchy)
        {
            transform.LookAt(vehicleTransform);
        }
    }
}
