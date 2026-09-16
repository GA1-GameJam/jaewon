using System.Collections;
using UnityEngine;

public class EffectAutoDespawn : MonoBehaviour, IPoolable
{
    [Header("생존 시간 (초)")]
    [SerializeField] private float _lifeTime = 0.5f;

    [Header("파티클 시스템 기반 자동 시간 계산")]
    [SerializeField] private bool _autoCalculateDuration = true;

    private ParticleSystem[] _particleSystems;
    private Coroutine _despawnCoroutine;

    private void Awake()
    {
        CacheParticleSystems();
        CalculateDuration();
    }

    private void CacheParticleSystems()
    {
        if (_particleSystems == null || _particleSystems.Length == 0)
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        }
    }

    private void CalculateDuration()
    {
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

    /// <summary>
    /// 폭발 파티클의 메인 색상을 동적으로 설정합니다.
    /// </summary>
    public void SetColor(Color color)
    {
        CacheParticleSystems();
        if (_particleSystems == null) return;

        foreach (var ps in _particleSystems)
        {
            // 연기(Smoke) 제외, 폭발 파티클만 색상 적용
            if (ps.gameObject.name.ToLower().Contains("smoke"))
            {
                continue;
            }

            var main = ps.main;
            main.startColor = color;
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
        CacheParticleSystems();

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
