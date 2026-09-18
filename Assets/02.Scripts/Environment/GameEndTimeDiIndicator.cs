using System.Collections;
using TMPro;
using UnityEngine;

public class GameEndTimeDiIndicator : MonoBehaviour
{

    [Header("텍스트")][SerializeField] private TextMeshProUGUI _text;
    [Header("타이머")][SerializeField] private Timer _timer;
    [Header("최고기록 갱신 텍스트")][SerializeField] private GameObject _alarmText;
    [Header("숫자 애니메이션 시간")][SerializeField] private float _countUpDuration = 1.2f;
    [Header("숫자 애니메이션 팝 크기")][SerializeField] private float _popScale = 1.08f;

    private float _maxTime;
    private Coroutine _countUpCoroutine;
    private Vector3 _originScale;

    private void Awake()
    {
        if (_text != null)
        {
            _originScale = _text.transform.localScale;
        }
    }

    private void OnEnable()
    {
        GetSaveTime();
        if (_text == null || _timer == null)
            return;

        int totalSeconds = Mathf.FloorToInt(_timer.GameTime);
        if (_originScale == Vector3.zero)
        {
            _originScale = _text.transform.localScale;
        }

        if (_countUpCoroutine != null)
        {
            StopCoroutine(_countUpCoroutine);
        }
        _countUpCoroutine = StartCoroutine(CountUpTimeText(totalSeconds));

        if (_timer.GameTime > _maxTime)
        {
            SetSaveTime();
            if (_alarmText != null)
            {
                _alarmText.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (_countUpCoroutine != null)
        {
            StopCoroutine(_countUpCoroutine);
            _countUpCoroutine = null;
        }

        if (_text != null)
        {
            _text.transform.localScale = _originScale;
        }
    }

    private IEnumerator CountUpTimeText(int targetSeconds)
    {
        float elapsed = 0f;
        int previousSeconds = -1;

        while (elapsed < _countUpDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / _countUpDuration);
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
            int currentSeconds = Mathf.FloorToInt(Mathf.Lerp(0f, targetSeconds, easedProgress));

            if (currentSeconds != previousSeconds)
            {
                SetTimeText(currentSeconds);
                previousSeconds = currentSeconds;
            }

            float pop = Mathf.Sin(progress * Mathf.PI) * (_popScale - 1f);
            _text.transform.localScale = _originScale * (1f + pop);
            yield return null;
        }

        SetTimeText(targetSeconds);
        _text.transform.localScale = _originScale;
        _countUpCoroutine = null;
    }

    private void SetTimeText(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        _text.text = $"{minutes:00}:{seconds:00}";
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
