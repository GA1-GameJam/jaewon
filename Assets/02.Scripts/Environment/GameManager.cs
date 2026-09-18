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

    private CameraShake _cameraShake;
    public CameraShake CameraShake => _cameraShake;
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
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            _cameraShake = mainCamera.GetComponent<CameraShake>();
            if (_cameraShake == null)
                _cameraShake = mainCamera.gameObject.AddComponent<CameraShake>();
        }
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
