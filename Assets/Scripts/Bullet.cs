using UnityEngine;

// Technique for making sure there isn't a null reference during runtime if you are going to use get component
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
    public float speed = 5;
    public AudioClip shootClip;

    //-----------------------------------------------------------------------------
    void Start()
    {
        Fire();
        GetComponent<AudioSource>().PlayOneShot(shootClip);
    }

    //-----------------------------------------------------------------------------
    private void Fire()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
    }
}
