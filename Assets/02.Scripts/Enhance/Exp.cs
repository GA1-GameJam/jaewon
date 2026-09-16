using UnityEngine;

public class Exp : MonoBehaviour, IPoolable
{
    [Header("경험치")]
    [SerializeField] private float _expAmount = 1f;

    [Header("플레이어 추적")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _waveAmplitude = 0.35f;
    [SerializeField] private float _waveFrequency = 8f;
    [SerializeField] private float _collectDistance = 0.2f;

    private Transform _player;
    private float _waveTime;
    private float _wavePhase;
    private float _previousWave;
    private bool _collected;


    private void Update()
    {
        if (_collected || !TryFindPlayer())
        {
            return;
        }

        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        Vector2 currentPosition = transform.position;
        Vector2 toPlayer = (Vector2)_player.position - currentPosition;
        float distance = toPlayer.magnitude;

        if (distance <= _collectDistance)
        {
            Collect();
            return;
        }

        Vector2 forward = toPlayer / distance;
        Vector2 perpendicular = new Vector2(-forward.y, forward.x);

        _waveTime += Time.deltaTime;
        float wave = Mathf.Sin(_wavePhase + _waveTime * _waveFrequency) * _waveAmplitude;
        float waveDelta = wave - _previousWave;
        _previousWave = wave;

        transform.position = (Vector3)currentPosition
            + (Vector3)(forward * (_moveSpeed * Time.deltaTime) + perpendicular * waveDelta);
    }

    private bool TryFindPlayer()
    {
        if (_player != null)
        {
            return true;
        }

        if (GameManager.Instance == null || GameManager.Instance.Player == null)
        {
            return false;
        }

        _player = GameManager.Instance.Player.transform;
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (_collected)
        {
            return;
        }

        _collected = true;

        if (EnhanceManager.Instance != null)
        {
            EnhanceManager.Instance.TakeExp(_expAmount);
        }

        if (PoolManager.Instance != null && PoolManager.Instance.ExpPoolFactory != null)
        {
            PoolManager.Instance.ExpPoolFactory.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Spawn()
    {
        _player = null;
        _waveTime = 0f;
        _wavePhase = Random.Range(0f, Mathf.PI * 2f);
        _previousWave = Mathf.Sin(_wavePhase) * _waveAmplitude;
        _collected = false;
    }

    public void Despawn()
    {
        _player = null;
        _collected = false;
    }
}
