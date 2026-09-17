using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{

    private EventTrigger _eventTrigger;
    
     private float _duration = 0.25f;
     private AnimationCurve _ease = null;
     private bool _openOnEnable;
    
    private Vector3 _openScale;
    private Coroutine _scaleCoroutine;

    private void Awake()
    {
        _eventTrigger = GetComponent<EventTrigger>();
        if (_eventTrigger == null)
        {
            _eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pointerEnter = new()
        {
            eventID = EventTriggerType.PointerEnter
        };
        pointerEnter.callback.AddListener(_ => StartHover());
        _eventTrigger.triggers.Add(pointerEnter);

        EventTrigger.Entry pointerExit = new()
        {
            eventID = EventTriggerType.PointerExit
        };
        pointerExit.callback.AddListener(_ => StopHover());
        _eventTrigger.triggers.Add(pointerExit);
        
        
        
        
        _openScale = transform.localScale;

        if (_ease == null || _ease.length == 0)
        {
            _ease = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        }
        Open();
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

    public void StartHover()
    {
        PlayScaleAnimation(_openScale * 1.1f);
    }

    public void StopHover()
    {
        PlayScaleAnimation(_openScale);
    }
}
