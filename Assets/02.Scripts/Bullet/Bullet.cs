using System;
using UnityEngine;

public enum BulletType
{
    normal,
}

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private BulletStat _bulletStat;
    public BulletStat BulletStat => _bulletStat;

    [Header("총알 색상")]
    [SerializeField] private Color _bulletColor = Color.white;
    public Color BulletColor => _bulletColor;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ApplyColor();
    }

    public void SetColor(Color color)
    {
        _bulletColor = color;
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _bulletColor;
        }
    }

    public void Spawn()
    {
        ApplyColor();
    }

    public void Despawn()
    {
        if (_bulletStat != null && _bulletStat.BulletDestroyVfx != null)
        {
            GameObject vfx = PoolManager.Instance.VfxPoolFactory.Get(_bulletStat.BulletDestroyVfx, transform.position, transform.rotation);
            if (vfx != null)
            {
                EffectAutoDespawn effect = vfx.GetComponent<EffectAutoDespawn>();
                if (effect != null)
                {
                    effect.SetColor(_bulletColor);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적에게 데미지 전달
            // other.GetComponent<Enemy>()?.TakeDamage(_bulletStat.BulletDamage);
        }

        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            PoolManager.Instance.BulletPoolFactory.Release(gameObject);
        }
    }
}
