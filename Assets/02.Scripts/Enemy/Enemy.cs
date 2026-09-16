using UnityEngine;

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
                EnhanceManager.Instance.CreateExp(transform.position);
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

        SpawnEvent();
        ApplyHealthMultiplier(1f);
    }

    private void SpawnEvent()
    {
        ObjectSquash objectSquash=GetComponent<ObjectSquash>();
        if (objectSquash != null)
        {
            objectSquash.StartSquash(0.5f);
        }

        ObjectRotator objectRotator = GetComponent<ObjectRotator>();
        if (objectRotator != null)
        {
            objectRotator.StartRotate(0.5f);
        }
    }
    public void ApplyHealthMultiplier(float multiplier)
    {
        if (_enemyStat == null)
        {
            return;
        }

        multiplier = Mathf.Max(0f, multiplier);
        _maxHP = _basicHealth * _enemyStat.Health * multiplier;
        _currentHP = _maxHP;

        if (_enemyColor != null)
        {
            _enemyColor.DecisionColorByHp(_maxHP, _currentHP);
        }
    }

    public void Despawn()
    {
    }
}
