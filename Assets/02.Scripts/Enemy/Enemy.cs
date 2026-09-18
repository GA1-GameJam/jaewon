using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    [Header("기본 체력")]
    [SerializeField] private float _basicHealth = 10f;
    [Header("가능한 최소 크기")]
    [SerializeField] private Vector3 _minScale = new Vector3(0.8f, 0.8f, 0.8f);
    [Header("가능한 최대 크기")]
    [SerializeField] private Vector3 _maxScale = new Vector3(1.2f, 1.2f, 1.2f);
    
    
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

        // 풀에서 생성되는 자식 적도 체력과 초기 색상을 반드시 초기화합니다.
        ApplyHealthMultiplier(1f);
        SetRandomScale();
        SpawnEvent();
        //ApplyHealthMultiplier(1f);
    }

    private void SetRandomScale()
    {
        float scale = Random.Range(0f, 1f);
        transform.localScale = Vector3.Lerp(_minScale, _maxScale, scale);
    }
    
    private void SpawnEvent()
    {
        ObjectSquash objectSquash=GetComponent<ObjectSquash>();
        if (objectSquash != null)
        {
            objectSquash.StartSquash();
        }

        ObjectRotator objectRotator = GetComponent<ObjectRotator>();
        if (objectRotator != null)
        {
            objectRotator.StartRotate();
        }
        
        ObjectScaler objectScaler = GetComponent<ObjectScaler>();
        if (objectScaler != null)
        {
            objectScaler.SizeUpStart();
        }
        
        ObjectViber objectViber = GetComponent<ObjectViber>();
        if (objectViber != null)
        {
            objectViber.StartVibe();
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
        AudioManager.Instance.PlayEffectClip(_enemyStat.EnemyDieAudioClip);
        DeSpawnEvent();
        MakeDeathVfx();
    }

    private void MakeDeathVfx()
    {
        
            GameObject vfx = PoolManager.Instance.VfxPoolFactory.Get(_enemyStat.EnemyDieVfx, transform.position, transform.rotation);
            if (vfx != null)
            {
                EffectAutoDespawn effect = vfx.GetComponent<EffectAutoDespawn>();
                if (effect != null)
                {
                    effect.SetColor(_enemyColor.GetEnemyColor);
                }
            }
        
        
    }
    private void DeSpawnEvent()
    {
        MakeEnemyChild makeEnemyChild=GetComponent<MakeEnemyChild>();
        if (makeEnemyChild != null)
        {
            makeEnemyChild.MakeChild();
        }
        
    }
}
