
using UnityEngine;

public class BrickSoundController : MonoBehaviour
{
    [SerializeField] private string[] brickSounds;

    private void Awake()
    {
        brickSounds = new string[]
        {
            Consts.Sounds.BRICK_SOUND1,
            Consts.Sounds.BRICK_SOUND2,
            Consts.Sounds.BRICK_SOUND3,
            Consts.Sounds.BRICK_SOUND4,
            Consts.Sounds.BRICK_SOUND5,
            Consts.Sounds.BRICK_SOUND6,
            Consts.Sounds.BRICK_SOUND7,
            Consts.Sounds.BRICK_SOUND8,
            Consts.Sounds.BRICK_SOUND9,
            Consts.Sounds.BRICK_SOUND10
        };
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Consts.Tags.PLAYER))
        {
            string randomBrickSound = brickSounds[Random.Range(0, brickSounds.Length)];
            AudioManager.Instance.Play(randomBrickSound);
        }
    }
}
