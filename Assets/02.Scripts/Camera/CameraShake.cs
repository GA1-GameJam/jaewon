using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class CameraShake : MonoBehaviour
{
    private Vector3 _shakeOffset;
    private Vector3 _appliedShakeOffset;
    private Coroutine _shakeCoroutine;

    private void LateUpdate()
    {
        // PlayerFollow가 카메라 위치를 갱신한 뒤 마지막에 흔들림을 적용합니다.
        transform.localPosition -= _appliedShakeOffset;
        transform.localPosition += _shakeOffset;
        _appliedShakeOffset = _shakeOffset;
    }

    public void Shake(float strength, float duration)
    {
        StartShake(strength, duration);
    }

    private void StartShake(float strength, float duration)
    {
        if (strength <= 0f || duration <= 0f)
            return;

        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);

        _shakeCoroutine = StartCoroutine(ShakeRoutine(strength, duration));
    }

    private IEnumerator ShakeRoutine(float strength, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            float fade = 1f - progress;
            _shakeOffset = Random.insideUnitCircle * strength * fade;
            yield return null;
        }

        _shakeOffset = Vector3.zero;
        _shakeCoroutine = null;
    }
}
