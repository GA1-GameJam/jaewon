using System.Collections;
using UnityEngine;

public class ObjectSquash : MonoBehaviour
{
    [Header("스쿼시 비율")]
    [SerializeField] private Vector3 _squashScale = new Vector3(1.2f, 0.8f, 1f);

    private Vector3 _originalScale;
    private Coroutine _squashCoroutine;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void StartSquash(float squashTime)
    {
        if (squashTime <= 0f)
        {
            transform.localScale = _originalScale;
            return;
        }

        if (_squashCoroutine != null)
        {
            StopCoroutine(_squashCoroutine);
        }

        transform.localScale = _originalScale;
        _squashCoroutine = StartCoroutine(SquashRoutine(squashTime));
    }

    // 기존 호출 이름과의 호환성을 유지한다.
    public void StartSquerse(float squashTime)
    {
        StartSquash(squashTime);
    }

    private IEnumerator SquashRoutine(float squashTime)
    {
        float halfTime = squashTime * 0.5f;
        float elapsed = 0f;

        while (elapsed < halfTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfTime);
            t = t * t * (3f - 2f * t);
            transform.localScale = Vector3.Lerp(_originalScale, ScaleFromRatio(), t);
            yield return null;
        }

        elapsed = 0f;
        Vector3 squashedScale = ScaleFromRatio();

        while (elapsed < halfTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfTime);
            t = t * t * (3f - 2f * t);
            transform.localScale = Vector3.Lerp(squashedScale, _originalScale, t);
            yield return null;
        }

        transform.localScale = _originalScale;
        _squashCoroutine = null;
    }

    private Vector3 ScaleFromRatio()
    {
        return Vector3.Scale(_originalScale, _squashScale);
    }

    private void OnDisable()
    {
        if (_squashCoroutine != null)
        {
            StopCoroutine(_squashCoroutine);
            _squashCoroutine = null;
        }

        transform.localScale = _originalScale;
    }
}
