using System.Collections;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private float _duration = 0.25f;
    [SerializeField] private AnimationCurve _ease = null;
    [SerializeField] private bool _openOnEnable;

    private Vector3 _openScale;
    private Coroutine _scaleCoroutine;

    private void Awake()
    {
        _openScale = transform.localScale;

        if (_ease == null || _ease.length == 0)
        {
            _ease = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        }
    }

    private void OnEnable()
    {
        if (_openOnEnable)
        {
            transform.localScale = Vector3.zero;
            Open();
        }
    }

    public void Open()
    {
        PlayScaleAnimation(_openScale);
    }

    public void Close()
    {
        PlayScaleAnimation(Vector3.zero);
    }

    private void PlayScaleAnimation(Vector3 targetScale)
    {
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
        }

        _scaleCoroutine = StartCoroutine(AnimateScale(targetScale));
    }

    private IEnumerator AnimateScale(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;
        float duration = Mathf.Max(0.01f, _duration);

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            float easedProgress = _ease.Evaluate(progress);
            transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, easedProgress);
            yield return null;
        }

        transform.localScale = targetScale;
        _scaleCoroutine = null;
        if (targetScale == Vector3.zero)
        {
            gameObject.SetActive(false);
        }
    }
}
