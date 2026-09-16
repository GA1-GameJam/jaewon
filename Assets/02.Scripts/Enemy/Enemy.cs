using UnityEngine;

public enum EnemyType
{
    Forward,
}

public class Enemy : MonoBehaviour, IPoolable
{
    [Header("기본 체력")]
    [SerializeField] private float _basicHealth = 10f;

    [SerializeField] private EnemyStat _enemyStat;
    public EnemyStat EnemyStat => _enemyStat;

    private EnemyColor _enemyColor;
    private float _maxHP;
    private float _currentHP;

    private void Awake()
    {
        _enemyColor = GetComponent<EnemyColor>();
    }

    public void TakeDamage(float input)
    {
        if (input <= 0f) return;

        Debug.Log(_currentHP);
        Debug.Log(input);
        _currentHP -= input;
        if (_currentHP <= 0f)
        {
            if (PoolManager.Instance != null && PoolManager.Instance.EnemyPoolFactory != null)
            {
                PoolManager.Instance.EnemyPoolFactory.Release(this.gameObject); // 사망
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (_enemyColor != null)
            {
                _enemyColor.DecisionColorByHp(_maxHP, _currentHP);
            }
        }
    }

    public void Spawn()
    {
        if (_enemyColor == null)
        {
            _enemyColor = GetComponent<EnemyColor>();
        }

        _maxHP = _basicHealth * _enemyStat.Health;
        _currentHP = _maxHP;
        Debug.Log(_currentHP);
        if (_enemyColor != null)
        {
            _enemyColor.DecisionColorByHp(_maxHP, _currentHP);
        }
    }

    public void Despawn()
    {
        // 풀에 반환될 때 필요한 초기화 작업 수행
    }
}
