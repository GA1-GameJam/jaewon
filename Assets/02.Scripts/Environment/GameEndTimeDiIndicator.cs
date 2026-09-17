using System;
using TMPro;
using UnityEngine;

public class GameEndTimeDiIndicator : MonoBehaviour
{

    [Header("텍스트")][SerializeField] private TextMeshProUGUI _text;
    [Header("타이머")][SerializeField] private Timer _timer;
    [Header("최고기록 갱신 텍스트")][SerializeField] private GameObject _alarmText;
    private float _maxTime;
    private void OnEnable()
    {
        GetSaveTime();
        if (_text == null || _timer == null)
            return;

        int totalSeconds = Mathf.FloorToInt(_timer.GameTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        _text.text = $"{minutes:00}:{seconds:00}";

        if (_timer.GameTime > _maxTime)
        {
            SetSaveTime();
            _alarmText.SetActive(true);
        }
    }

    private void GetSaveTime()
    {
        _maxTime=PlayerPrefs.GetFloat("GameTime");
    }
    private void SetSaveTime()
    {
        PlayerPrefs.SetFloat("GameTime",_timer.GameTime);
    }
}
