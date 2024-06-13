using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoshootArea : MonoBehaviour
{
    public event Action OnPhotoshoot;

    [Header("References")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject photoshootCamera;

    [Header("Settings")]
    [SerializeField] private float waitingSeconds;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag(Consts.Tags.PHOTOSHOOT_AREA))
        {
            SetCameras(photoshootCamera, mainCamera);
            StartCoroutine(OnPhotoshootCoroutine());
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(Consts.Tags.PHOTOSHOOT_AREA))
        {
            SetCameras(mainCamera, photoshootCamera);
        }   
    }

    private void SetCameras(GameObject activeCamera, GameObject passiveCamera)
    {
        activeCamera.SetActive(true);
        passiveCamera.SetActive(false);
    }

    private IEnumerator OnPhotoshootCoroutine()
    {
        yield return new WaitForSeconds(waitingSeconds);
        OnPhotoshoot?.Invoke();
    }
}
