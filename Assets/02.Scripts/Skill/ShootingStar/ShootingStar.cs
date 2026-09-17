using UnityEngine;

public class ShootingStar : SkillParent
{
    [Header("별 프리팹 ")][SerializeField] private GameObject _shootingStarPrefab;
    [Header("소환 쿨타임 ")][SerializeField] private float _spawnCooltime=10f;
    [Header("레벨업별 증가 개수 ")][SerializeField] private int _spawnPlusCount=5;

    [Header("소환 개수 ")][SerializeField] private int _spawnCount=10;
    [Header("각성시 소환 개수 ")][SerializeField] private int _awakeningSpawnCount=5;
    [Header("각성시 소환 쿨타임 ")][SerializeField] private float _awakeningSpawnCoolTime=3;

    [Header("소환 거리 ")][SerializeField] private float _spawnDistance=20f;
    [Header("플레이어 주변 목표 범위 ")][SerializeField] private float _targetRadius=2f;
    
    private float _currentTime;
    private Player _player;
    void Start()
    {
        _player=GameManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime-= Time.deltaTime;
        if (_currentTime < 0)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        GameManager.Instance.CameraShake.Shake(0.2f,0.2f);

        if (_player == null || _shootingStarPrefab == null ||
            PoolManager.Instance == null || PoolManager.Instance.BulletPoolFactory == null)
        {
            return;
        }

        _currentTime = Mathf.Max(0.01f, _spawnCooltime);

        for (int i = 0; i < _spawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            Quaternion rotation = GetRandomRotation(spawnPosition);

            PoolManager.Instance.BulletPoolFactory.Get(
                _shootingStarPrefab,
                spawnPosition,
                rotation);

        }
    }

    private Vector3 GetRandomPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        return _player.transform.position + direction * _spawnDistance;
    }

    private Quaternion GetRandomRotation(Vector3 spawnPosition)
    {
        Vector2 targetOffset = Random.insideUnitCircle * _targetRadius;
        Vector3 targetPosition = _player.transform.position + (Vector3)targetOffset;
        Vector3 direction = targetPosition - spawnPosition;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
        {
            return Quaternion.identity;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, angle - 90f);
    }

    protected override void ExcuteLevelUp()
    {
        _spawnCount += _spawnPlusCount;
    }

    protected override void Awakening()
    {
        _spawnCooltime = _awakeningSpawnCoolTime;
        _spawnCount = _awakeningSpawnCount;
    }
}
