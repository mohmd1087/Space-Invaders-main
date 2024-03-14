
using System.Collections;
using UnityEngine;

using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SpawnerController : MonoBehaviour
{

    public GameObject EnemyTopPrefab;
    public GameObject EnemyMiddlePrefab;
    public GameObject EnemyBottomPrefab;
    public GameObject EnemyRandomPrefab;
    public GameObject EnemyParent;
    public Transform EnemyRandomSpawn;

    public float dropCoolDownSeconds = 3f;
    public float downwardMovementMult = 0.5f;
    public float baseSpeedPerSecond = 0.1f;
    public float enemyDestroyedSpeedMult = 0.01f;

    public static int EnemiesSpawned;

    private int _enemiesPerRow;
    private bool _onCoolDown;
    private float _distanceBetweenEnemies;
    private float _endDownTime;
    private float _currentSpeed;
    private Transform _rootTransform;
    private Rigidbody2D _rigidbody2D;

    private static bool _movingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        _enemiesPerRow = 11;
        _distanceBetweenEnemies = 0.75f;
        _currentSpeed = baseSpeedPerSecond;
        _onCoolDown = false;
        _rootTransform = EnemyParent.transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Border.OnEnemyHitBorder += OnBorderHit;
        EnemyComplete.OnEnemyAboutToBeDestroyed += OnEnemyDestroyed;
        Player.OnPlayerOutOfLives += OnGameOver;
        SpawnEnemies();
        StartCoroutine(RandomCoroutine());

        EnemiesSpawned = _enemiesPerRow * 5;
    }

    private void FixedUpdate()
    {
        if (_movingRight)
        {
            _rigidbody2D.velocity = Vector2.right * _currentSpeed;
        }
        else
        {
            _rigidbody2D.velocity = Vector2.left * _currentSpeed;
        }

        if (_onCoolDown && Time.realtimeSinceStartup >= _endDownTime)
        {
            _onCoolDown = false;
            _endDownTime = 0f;
        }
    }

    private void OnDestroy()
    {
        Border.OnEnemyHitBorder -= OnBorderHit;
        EnemyComplete.OnEnemyAboutToBeDestroyed -= OnEnemyDestroyed;
        Player.OnPlayerOutOfLives -= OnGameOver;
    }

    IEnumerator RandomCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(8f);
            Instantiate(EnemyRandomPrefab, new Vector3(EnemyRandomSpawn.position.x, EnemyRandomSpawn.position.y, 0), Quaternion.identity);
        }
    }

    private void OnBorderHit()
    {
        if (_onCoolDown)
            return;
        
        // not on cool down
        _onCoolDown = true;
        _endDownTime = Time.realtimeSinceStartup + dropCoolDownSeconds;
        
        MoveDown();
    }

    private void OnEnemyDestroyed(int score)
    {
        _currentSpeed += enemyDestroyedSpeedMult;
    }

    private void OnGameOver()
    {
        Destroy(gameObject);
    }

    private void MoveDown()
    {
        transform.position += Vector3.down * downwardMovementMult;
    }

    // todo: reformat if time permits
    private void SpawnEnemies()
    {
        Vector3 spawnerLocation = transform.position; // will change based on rounds completed
        
        // Top row
        for (float x = 0; x < _enemiesPerRow * _distanceBetweenEnemies; x += _distanceBetweenEnemies)
        {
            Instantiate(EnemyTopPrefab, new Vector3(spawnerLocation.x + x, spawnerLocation.y, 0), Quaternion.identity, _rootTransform);
        }
        
        // Middle Rows
        for (float y = -_distanceBetweenEnemies; y >= -2 * _distanceBetweenEnemies; y -= _distanceBetweenEnemies)
        {
            for (float x = 0; x < _enemiesPerRow * _distanceBetweenEnemies; x += _distanceBetweenEnemies)
            {
                Instantiate(EnemyMiddlePrefab, new Vector3(spawnerLocation.x + x, spawnerLocation.y + y, 0), Quaternion.identity, _rootTransform);
            }
        }
        // Bottom Rows
        for (float y = -3 * _distanceBetweenEnemies; y >= -4 * _distanceBetweenEnemies; y -= _distanceBetweenEnemies)
        {
            for (float x = 0; x < _enemiesPerRow * _distanceBetweenEnemies; x += _distanceBetweenEnemies)
            {
                Instantiate(EnemyBottomPrefab, new Vector3(spawnerLocation.x + x, spawnerLocation.y + y, 0), Quaternion.identity, _rootTransform);
            }
        }
    }

    public static bool IsMovingRight()
    {
        return _movingRight;
    }

    public static void SetMovingRight(bool isMovingRight)
    {
        _movingRight = isMovingRight;
    }
}
