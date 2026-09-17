using UnityEngine;

public class ObjectViber : MonoBehaviour
{
    [Header("진동 거리")]
    [SerializeField] private float _vibeDistance = 0.1f;

    [Header("진동 속도")]
    [SerializeField] private float _vibeSpeed = 8f;

    private bool _isVibeStarted;
    private Vector3 _originalLocalPosition;
    private float _vibeTime;

    private void Awake()
    {
        _originalLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_isVibeStarted)
        {
            Vibe();
        }
    }

    public void StartVibe()
    {
        _originalLocalPosition = transform.localPosition;
        _vibeTime = 0f;
        _isVibeStarted = true;
    }

    public void StopVibe()
    {
        _isVibeStarted = false;
        transform.localPosition = _originalLocalPosition;
    }

    private void Vibe()
    {
        _vibeTime += Time.deltaTime;

        float offset = Mathf.Sin(_vibeTime * _vibeSpeed) * _vibeDistance;
        transform.localPosition = _originalLocalPosition + Vector3.up * offset;
    }

    private void OnDisable()
    {
        StopVibe();
    }
}
