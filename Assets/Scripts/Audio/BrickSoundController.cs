using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSoundController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag(Consts.Tags.PLAYER))
        {
            Debug.Log("Player!");
            AudioManager.Instance.Play(Consts.Sounds.BRICK_SOUND);
        }    
    }
}
