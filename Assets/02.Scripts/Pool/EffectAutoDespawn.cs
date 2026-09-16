using System.Collections;
using UnityEngine;

public class EffectAutoDespawn : MonoBehaviour, IPoolable
{
    [Header("생존 시간 (초)")]
    [SerializeField] private float _lifeTime = 2.5f;

    [Header("파티클 시스템 기반 자동 시간 계산")]
    [SerializeField] private bool _autoCalculateDuration = true;

    private ParticleSystem[] _particleSystems;
    private Coroutine _despawnCoroutine;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        if (_autoCalculateDuration && _particleSystems != null && _particleSystems.Length > 0)
        {
            float maxDuration = 0f;
            foreach (var ps in _particleSystems)
            {
                var main = ps.main;
                float total = main.duration + main.startLifetime.constantMax;
                if (total > maxDuration && main.startLifetime.constantMax < 20f)
                {
                    maxDuration = total;
                }
            }
            if (maxDuration > 0f)
            {
                _lifeTime = maxDuration;
            }
        }
    }

    private void OnEnable()
    {
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
        }
        _despawnCoroutine = StartCoroutine(DespawnRoutine());
    }

    private void OnDisable()
    {
        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
            _despawnCoroutine = null;
        }
    }

    public void Spawn()
    {
        if (_particleSystems == null || _particleSystems.Length == 0)
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        }

        foreach (var ps in _particleSystems)
        {
            ps.Clear(true);
            ps.Play(true);
        }

        if (gameObject.activeInHierarchy)
        {
            if (_despawnCoroutine != null)
            {
                StopCoroutine(_despawnCoroutine);
            }
            _despawnCoroutine = StartCoroutine(DespawnRoutine());
        }
    }

    public void Despawn()
    {
        if (_particleSystems != null)
        {
            foreach (var ps in _particleSystems)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    private IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        DespawnSelf();
    }

    private void DespawnSelf()
    {
        if (PoolManager.Instance != null && PoolManager.Instance.VfxPoolFactory != null)
        {
            PoolManager.Instance.VfxPoolFactory.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
