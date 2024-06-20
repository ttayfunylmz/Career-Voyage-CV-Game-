using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private string soundName;

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag(Consts.Tags.PLAYER))
        {
            AudioManager.Instance.Play(soundName);
        }    
    }
}
