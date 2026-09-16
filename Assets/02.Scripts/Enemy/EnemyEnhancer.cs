using System.Collections.Generic;
using UnityEngine;

public class EnemyEnhancer : MonoBehaviour
{

    [Header("소환 가능한 적 종류")] [SerializeField]
    private List<GameObject> _enemyPrefabs;
    [Header("적 강화까지 시간차")] [SerializeField]
    private float _enhanceTime=10.0f;
    [Header("체력 강화 배율")]
    [SerializeField] private float _healthEnhanceRate = 0.2f;

    private EnemySpawner _enemySpawner;
    
    private float _curTime=0;
    private int _enemyListIdx=0;
    private float _enemyHealthMultiplier = 1f;
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        _curTime -= Time.deltaTime;
        if (_curTime <= 0)
        {
            EnhanceEnemy();
        }
    }

    private void EnhanceEnemy()
    {
        _curTime = _enhanceTime;
        EnhanceEnemyStat();
        AddNewTypeEnemy();
        Debug.Log("적이 강화됩니다");
    }

    private void AddNewTypeEnemy()
    {
        if (_enemyListIdx <= _enemyPrefabs.Count - 1)
        {
            _enemySpawner.AddSpawnableEnemy(_enemyPrefabs[_enemyListIdx]);
            _enemyListIdx++;
        }
    }

    private void EnhanceEnemyStat()
    {
        _enemyHealthMultiplier += _healthEnhanceRate;
        _enemySpawner.SetEnemyHealthMultiplier(_enemyHealthMultiplier);
    }
    
    
    
}
