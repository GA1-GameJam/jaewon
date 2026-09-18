using UnityEngine;

public class MakeEnemyChild : MonoBehaviour
{
    [Header("생성될 자식 오브젝트 프리팹")][SerializeField]private Enemy _enemy;

    [Header("자식 수")]
    [SerializeField] private int _count=6;

    [Header("자동 자식 생성")]
    [SerializeField] private bool _isAutoSpawn;
    [Header("자동 자식 생성 쿨타임")]
    [SerializeField] private float _autoSpawnTime=1f;
    private float _currentTime;

    private void Update()
    {
        if (_isAutoSpawn)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0f)
            {
                _currentTime = _autoSpawnTime;
                MakeChild();
            }
        }
    }
    
    
    public void MakeChild()
    {
        if (_enemy == null)
        {
            Debug.LogWarning($"{nameof(MakeEnemyChild)}: 생성할 Enemy 프리팹이 할당되지 않았습니다.");
            return;
        }

        if (PoolManager.Instance == null || PoolManager.Instance.EnemyPoolFactory == null)
        {
            Debug.LogWarning($"{nameof(MakeEnemyChild)}: EnemyPoolFactory를 찾을 수 없습니다.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 1.5f;

            PoolManager.Instance.EnemyPoolFactory.Get(
                _enemy,
                transform.position + (Vector3)offset,
                Quaternion.identity);
        }
    }
}
