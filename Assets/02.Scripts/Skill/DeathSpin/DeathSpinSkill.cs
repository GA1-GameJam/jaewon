using System.Collections.Generic;
using UnityEngine;

public class DeathSpinSkill : SkillParent
{
    [Header("스킬쿨타임")][SerializeField] private float _skillCoolTime=2;
    [Header("생성될 가시프리팹")][SerializeField]private GameObject _deathSpinPrefab;
    private int _spawnCount=1;    
    
    [Header("각성시 스킬쿨타임")][SerializeField] private float _awakenSkillCoolTime=2;
    [Header("생성될 가시프리팹")][SerializeField]private GameObject _awakenDeathSpinPrefab;
    private float _curTime;
    private Player _player;
    private readonly HashSet<Enemy> _selectedEnemies = new();

    void Start()
    {
        _player=GameManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        _curTime-=Time.deltaTime;
        if (_curTime <= 0)
        {
            _curTime = _skillCoolTime;
            SpawnDeathSpin();
        }
    }

    private void SpawnDeathSpin()
    {
        GameManager.Instance.CameraShake.Shake(0.2f,0.2f);

        if (_deathSpinPrefab == null || _player == null)
        {
            return;
        }

        _selectedEnemies.Clear();

        for (int i = 0; i < _spawnCount; i++)
        {
            if (!TryGetNearestEnemyToPlayer(out Vector3 spawnPosition))
            {
                break;
            }

            GameObject deathSpin=Instantiate(_deathSpinPrefab, spawnPosition, Quaternion.identity);
            deathSpin.GetComponent<SkillColor>().SetColor(_level);
        }
    }

    private bool TryGetNearestEnemyToPlayer(out Vector3 spawnPosition)
    {
        Enemy nearestEnemy = null;
        float nearestDistanceSqr = float.MaxValue;
        Vector3 playerPosition = _player.transform.position;

        foreach (GameObject enemyObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemyObject == null)
            {
                continue;
            }

            Enemy enemy = enemyObject.GetComponent<Enemy>();
            if (enemy == null || _selectedEnemies.Contains(enemy))
            {
                continue;
            }

            float distanceSqr = (enemy.transform.position - playerPosition).sqrMagnitude;
            if (distanceSqr < nearestDistanceSqr)
            {
                nearestEnemy = enemy;
                nearestDistanceSqr = distanceSqr;
            }
        }

        if (nearestEnemy == null)
        {
            spawnPosition = default;
            return false;
        }

        _selectedEnemies.Add(nearestEnemy);
        spawnPosition = nearestEnemy.transform.position;
        return true;
    }

    protected override void ExcuteLevelUp()
    {
        _spawnCount++;
    }

    protected override void Awakening()
    {
        _skillCoolTime = _awakenSkillCoolTime;
    }
}
