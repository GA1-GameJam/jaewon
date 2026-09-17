using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletBurst : SkillParent
{
    [Header("총알 개수 ")][SerializeField]private int _burstBulletCount=12;
    [Header("사용 쿨타임")][SerializeField]private float _burstCoolTime=3;
    [Header("각성 시 쿨감 감소율")][SerializeField]private float _awakeningRate=0.5f;
    [Header("레벨별 총알 리스트 ")][SerializeField]private List<Bullet> _bullets;

    private float _curTime;
    private Player _player;

    private void Start()
    {
        _player = GameManager.Instance.Player;
    }
    private void Update()
    {
        _curTime -= Time.deltaTime;
        if (_curTime <= 0)
        {
            _curTime=_burstCoolTime;
            Burst();
        }
        
        SyncToPlayer();
    }

    private void SyncToPlayer()
    {
        transform.position = _player.transform.position;
    }

    private void Burst()
    {
        GameManager.Instance.CameraShake.Shake(0.2f,0.2f);
        if (_player == null || PoolManager.Instance == null ||
            PoolManager.Instance.BulletPoolFactory == null)
        {
            return;
        }

        if (_bullets == null || _bullets.Count == 0)
        {
            Debug.LogWarning($"{nameof(BulletBurst)}: 현재 레벨에 사용할 총알 프리팹이 없습니다.");
            return;
        }

        int bulletIndex = Mathf.Clamp(_level - 1, 0, _bullets.Count - 1);
        if (_bullets[bulletIndex] == null)
        {
            Debug.LogWarning($"{nameof(BulletBurst)}: 현재 레벨에 사용할 총알 프리팹이 없습니다.");
            return;
        }

        float angleStep = 360f / Mathf.Max(1, _burstBulletCount);

        for (int i = 0; i < _burstBulletCount; i++)
        {
            float angle = angleStep * i;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            PoolManager.Instance.BulletPoolFactory.Get(
                _bullets[bulletIndex],
                _player.transform.position,
                rotation);
        }
    }

    protected override void ExcuteLevelUp()
    {
    }

    protected override void Awakening()
    {
        _burstBulletCount *= 2;
        _burstCoolTime *= _awakeningRate;
    }
    
    
}
