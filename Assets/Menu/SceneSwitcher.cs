
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SceneSwitcher : MonoBehaviour
{
    public string gameSceneName;
    public string creditsSceneName;
    public string menuSceneName;
    public AudioClip uiClip;

    private AudioSource _source;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _source = GetComponent<AudioSource>();
        ScoreManager.OnGameOver += DelayedLoadCreditsScene;
    }

    private void OnDestroy()
    {
        ScoreManager.OnGameOver -= DelayedLoadCreditsScene;
    }

    public void LoadGameScene()
    {
        _source.PlayOneShot(uiClip);
        StartCoroutine(LoadAndSetup(gameSceneName));
    }

    public void LoadCreditsScene()
    {
        _source.PlayOneShot(uiClip);
        StartCoroutine(LoadAndSetup(creditsSceneName));
        StartCoroutine(DelayedLoadMenuScene());
    }

    public void LoadMenuScene()
    {
        _source.PlayOneShot(uiClip);
        StartCoroutine(LoadAndSetup(menuSceneName));
    }
    
    public void DelayedLoadCreditsScene()
    {
        StartCoroutine(GameEnded());
    }

    IEnumerator GameEnded()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
        LoadCreditsScene();
    }

    IEnumerator DelayedLoadMenuScene()
    {
        yield return new WaitForSeconds(10f);
        LoadMenuScene();
    }

    IEnumerator LoadAndSetup(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        
        if (sceneName.Equals("Menu"))
            Destroy(gameObject);
        
        yield return new WaitForSeconds(0.5f);
    }
}
