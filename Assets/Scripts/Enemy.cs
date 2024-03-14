
using System.Collections;

using UnityEngine;

using Random = UnityEngine.Random;

public class EnemyComplete : MonoBehaviour
{

    public int scorePerHit = 0;
    public float randomEnemySpeed = 3f;
    public float castOffset = 0.25f;
    public GameObject bulletPrefab;
    public AudioClip destroyedClip;

    public delegate void EnemyDestroyed(int score);
    public static event EnemyDestroyed OnEnemyAboutToBeDestroyed;

    private Animator _animator;
    private AudioSource _source;
    private bool _isRandomEnemy;

    private static float _shootChance = 0.1f;

    private void Start()
    {
        _isRandomEnemy = CompareTag("Random");
        _source = GetComponent<AudioSource>();
        if (!_isRandomEnemy)
        {
            _animator = GetComponent<Animator>();
            return;
        }

        GetComponent<Rigidbody2D>().velocity = Vector2.right * randomEnemySpeed;
    }

    private void OnDestroy()
    {
        if (!_isRandomEnemy)
            ScoreManager.EnemiesHit++;
    }

    private void Update()
    {
        TryShoot();
    }

    private void TryShoot()
    {
        if (_isRandomEnemy)
            return;

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position + Vector3.down*castOffset, Vector2.down, 8f);
        if (hitInfo.collider != null && !hitInfo.collider.CompareTag("Enemy"))
        {
            Debug.DrawLine(transform.position + Vector3.down * castOffset, transform.position + Vector3.down * 8f, Color.magenta);
            if (Random.Range(0f, 500f) <= _shootChance)
            {
                Instantiate(bulletPrefab, transform.position + Vector3.down, Quaternion.identity);
                _animator.SetTrigger("Shooting");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        _source.PlayOneShot(destroyedClip);
        
        if (_isRandomEnemy)
            OnEnemyAboutToBeDestroyed(Random.Range(3, 8) * 50);
        else
        {
            OnEnemyAboutToBeDestroyed(scorePerHit);
            _animator.SetTrigger("EnemyDying");
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject); // destroy bullet
            StartCoroutine(DelayDestroy(_isRandomEnemy ? 0.1f : 0.4f)); // destroy self
        }
    }

    IEnumerator DelayDestroy(float secondsDelay)
    {
        yield return new WaitForSeconds(secondsDelay);
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_isRandomEnemy && other.CompareTag("RightBorder"))
            Destroy(gameObject);
    }

    public static void SetShootChance(float newChance)
    {
        _shootChance = newChance;
    }
}
