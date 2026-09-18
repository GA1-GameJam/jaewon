using System.Collections;
using UnityEngine;

public class GeometryBarrier : SkillParent
{
    [Header("재활성화 시간")][SerializeField]private float _cooldown=5;
    [Header("레벨 별 크기 하양율")][SerializeField]private float _scaleMultiplier=0.2f;
    [Header("피격 시 축소 시간")][SerializeField] private float _scaleDownDuration = 0.15f;
    [Header("복구 시간")][SerializeField] private float _scaleUpDuration = 1f;
    [Header("공격시 이펙트")]
    [SerializeField] private GameObject _hitEffect;
    private Vector3 _currentScale;
    private Player _player;
    private Coroutine _scaleCoroutine;

    private void Awake()
    {
        _currentScale = transform.localScale;
    }

    private void Start()
    {
        _player=GameManager.Instance.Player;
    }

    private void Update()
    {
        SyncToPlayer();
    }

    private void SyncToPlayer()
    {
        transform.position=_player.transform.position;
    }

    protected override void ExcuteLevelUp()
    {
        StartScaleDownToNextLevel();
    }
    
    protected override void Awakening()
    {
        _cooldown *= 0.5f;
    }

    private void StartScaleDownToZero()
    {
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
        }

        _scaleCoroutine = StartCoroutine(ScaleDownAndRecover());
    }

    private void StartScaleUpToFit()
    {
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
        }

        _scaleCoroutine = StartCoroutine(ScaleToTarget(
            transform.localScale,
            _currentScale,
            _scaleUpDuration));
    }

    private void StartScaleDownToNextLevel()
    {
        float decrease = Mathf.Max(0f, _scaleMultiplier);
        Vector3 targetScale = new Vector3(
            Mathf.Max(0.01f, _currentScale.x - decrease),
            Mathf.Max(0.01f, _currentScale.y - decrease),
            Mathf.Max(0.01f, _currentScale.z - decrease));

        _currentScale = targetScale;
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
        }

        _scaleCoroutine = StartCoroutine(ScaleToTarget(
            transform.localScale,
            targetScale,
            _scaleUpDuration));
    }

    private IEnumerator ScaleDownAndRecover()
    {
        yield return ScaleToTarget(
            transform.localScale,
            Vector3.zero,
            _scaleDownDuration);

        transform.localScale = Vector3.zero;

        if (_cooldown > 0f)
        {
            yield return new WaitForSeconds(_cooldown);
        }

        yield return ScaleToTarget(
            Vector3.zero,
            _currentScale,
            _scaleUpDuration);

        transform.localScale = _currentScale;
        _scaleCoroutine = null;
    }

    private IEnumerator ScaleToTarget(
        Vector3 startScale,
        Vector3 targetScale,
        float duration)
    {
        float elapsed = 0f;
        duration = Mathf.Max(0.01f, duration);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            progress = progress * progress * (3f - 2f * progress);
            transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        transform.localScale = targetScale;
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            PoolManager.Instance.EnemyPoolFactory.Release(other.gameObject);
            GameManager.Instance.CameraShake.Shake(0.2f,0.2f);
            GameObject vfx = PoolManager.Instance.VfxPoolFactory.Get(_hitEffect, transform.position, transform.rotation);
            
            
            StartScaleDownToZero();
        }
    }
}
