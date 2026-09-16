using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _minSpawnDistance = 5f;
    [SerializeField] private float _maxSpawnDistance = 10f;

    private float _spawnTimer;

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Transform player=GameManager.Instance.Player.transform;
        // 0~360도 중 랜덤한 방향
        float angle = Random.Range(0f, 360f);

        // 플레이어로부터 랜덤한 거리
        float distance = Random.Range(_minSpawnDistance, _maxSpawnDistance);

        // 원 둘레의 위치 계산
        Vector2 direction = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        Vector2 spawnPosition = (Vector2)player.position + direction * distance;

        Vector2 lookDirection = (Vector2)player.position - spawnPosition;

        float rotationZ = Mathf.Atan2(
            lookDirection.y,
            lookDirection.x
        ) * Mathf.Rad2Deg - 90f;
        
        PoolManager.Instance.EnemyPoolFactory.Get(_enemyPrefab,spawnPosition,Quaternion.Euler(0f, 0f, rotationZ));

       

    }
}
