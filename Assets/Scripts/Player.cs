
using System.Collections;
using TMPro;
using UnityEngine;


public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootOffsetTransform;
    public TextMeshProUGUI livesText;
    public AudioClip destroyClip;
    public float unitsPerSecond = 15f;
    public int initialLives = 3;

    public delegate void PlayerOutOfLives();
    public static event PlayerOutOfLives OnPlayerOutOfLives;

    private Animator _animator;
    private Rigidbody2D _rigidBody2D;
    private AudioSource _source;
    private Vector3 _respawnPosition;
    private int _livesRemaining;
    private bool _inputIsLocked;
    
    //-----------------------------------------------------------------------------
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
        _respawnPosition = transform.position;
        
        _livesRemaining = initialLives;
        _inputIsLocked = false;
        
        livesText.text = $"Lives remaining: {initialLives}";
        
        _animator.SetBool("LivesRemain", true);
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
        // Shoot
        if (Input.GetKeyDown(KeyCode.Space) && !_inputIsLocked)
        {
            ShotRequested();
        }
    }

    private void FixedUpdate()
    {
        if (_inputIsLocked)
            return;
        
        _rigidBody2D.velocity = Input.GetAxis("Horizontal") * unitsPerSecond * Vector2.right;
    }

    private void ShotRequested()
    {
        // playerAnimator.SetTrigger("ShootTrigger");
        if (shootOffsetTransform.hierarchyCount != 2)
            return;
        
        _animator.SetTrigger("Shooting");
        
        GameObject shot = Instantiate(bulletPrefab, shootOffsetTransform.position, Quaternion.identity, shootOffsetTransform);

        Destroy(shot, 2f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Enemy"))
            return;
        
        Destroy(col.gameObject);

        _rigidBody2D.velocity = Vector2.zero;
        _livesRemaining--;
        
        _inputIsLocked = true;
        _source.PlayOneShot(destroyClip);
        _animator.SetTrigger("PlayerDying");
        
        if (_livesRemaining < 0) // game ends
        {
            _animator.SetBool("LivesRemain", false);
            OnPlayerOutOfLives();
            Destroy(gameObject);
        }
        else // respawn animation and sequencing
        {
            livesText.text = $"Lives remaining: {_livesRemaining}";
            StartCoroutine(DelayInputUnlock());
        }
    }

    IEnumerator DelayInputUnlock()
    {
        yield return new WaitForSeconds(1.25f);
        transform.position = _respawnPosition;
        _inputIsLocked = false;
    }
}
