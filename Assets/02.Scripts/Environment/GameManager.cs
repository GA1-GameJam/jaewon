using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance=>_instance;

    [Header("플레이어 참조")][SerializeField]private Player _player;
    [Header("GameOverPanel")][SerializeField]private GameObject _gameOverPanel;
    [Header("배경음악소스")][SerializeField]private  AudioSource _backgroundMusic;

    public Player Player => _player;
    [Header("클릭시 소환 vfx")][SerializeField]private GameObject _clickVfx;
    public GameObject ClickVfx => _clickVfx;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        if (_backgroundMusic != null)
            StartCoroutine(FadeOutBackgroundMusic(0.75f));

        Time.timeScale = 0.2f;
        _gameOverPanel.SetActive(true);
    }

    private IEnumerator FadeOutBackgroundMusic(float duration)
    {
        if (_backgroundMusic == null) yield break;

        float startVolume = _backgroundMusic.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            _backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        _backgroundMusic.volume = 0f;
        _backgroundMusic.Stop();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
