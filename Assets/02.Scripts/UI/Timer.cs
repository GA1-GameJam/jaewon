using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private float _gameTime;
    public float GameTime=>_gameTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        RefreshText();
    }

    // Update is called once per frame
    void Update()
    {
        _gameTime += Time.deltaTime;
        RefreshText();
    }

    private void RefreshText()
    {
        if (_text == null)
        {
            return;
        }

        int totalSeconds = Mathf.FloorToInt(_gameTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        _text.text = $"{minutes:00}:{seconds:00}";
    }
}
