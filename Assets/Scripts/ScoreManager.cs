using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverText;

    private int _score;
    
    public static int EnemiesHit;
    
    public delegate void GameOver();
    public static event GameOver OnGameOver;
    
    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        _score = 0;
        EnemiesHit = 0;
        if (!PlayerPrefs.HasKey("high score"))
            PlayerPrefs.SetInt("high score", _score);

        var oldHighScoreFormatted = PlayerPrefs.GetInt("high score").ToString("0000");
        highScoreText.text = $"HI-SCORE\n{oldHighScoreFormatted}";
        
        EnemyComplete.OnEnemyAboutToBeDestroyed += EnemyOnOnEnemyAboutToBeDestroyed;
        Player.OnPlayerOutOfLives += ScoreOnPlayerOutOfLives;
    }

    private void OnDestroy()
    {
        EnemyComplete.OnEnemyAboutToBeDestroyed -= EnemyOnOnEnemyAboutToBeDestroyed;
        Player.OnPlayerOutOfLives -= ScoreOnPlayerOutOfLives;
    }

    private void EnemyOnOnEnemyAboutToBeDestroyed(int score)
    {
        _score += score;
        var formattedScore = _score.ToString("0000");
        
        scoreText.text = $"SCORE\n{formattedScore}";
        CheckForEndGame();
    }

    private void ScoreOnPlayerOutOfLives()
    {
        EndGame();
    }

    private void CheckForEndGame()
    {
        if (EnemiesHit == SpawnerController.EnemiesSpawned)
            EndGame();
    }

    private void EndGame()
    {
        var oldHighScore = PlayerPrefs.GetInt("high score");
        var sessionScore = _score;

        if (oldHighScore < sessionScore)
        {
            PlayerPrefs.SetInt("high score", sessionScore);
            var newHighScoreFormatted = sessionScore.ToString("0000");
            highScoreText.text = $"HI-SCORE\n{newHighScoreFormatted}";
        }
    }
}
