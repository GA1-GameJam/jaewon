using System;
using UnityEngine;


public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private BulletStat _bulletStat;
    public BulletStat BulletStat => _bulletStat;
   
   private BulletColor _bulletColor; 
   private BulletMove _bulletMove;
   private int _remainHitCount;

    private void Awake()
    {
        _bulletMove = GetComponent<BulletMove>();
        _bulletColor=GetComponent<BulletColor>();
    }



    public void Spawn()
    {
        _remainHitCount = _bulletStat.HitEnableCount;
        _bulletColor.DecisionColorToAttackDamage();
    }

    public void Despawn()
    {
        AudioManager.Instance.PlayEffectClip(_bulletStat.BulletDestroySound);
        if (_bulletStat != null && _bulletStat.BulletDestroyVfx != null)
        {
            GameObject vfx = PoolManager.Instance.VfxPoolFactory.Get(_bulletStat.BulletDestroyVfx, transform.position, transform.rotation);
            if (vfx != null)
            {
                EffectAutoDespawn effect = vfx.GetComponent<EffectAutoDespawn>();
                if (effect != null)
                {
                    effect.SetColor(_bulletColor.GetBulletColor);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()?.TakeDamage(_bulletStat.BulletDamage*GameManager.Instance.Player.PlayerStat.AttackDamage);
            AudioManager.Instance.PlayEffectClip(_bulletStat.BulletHitSound);

            MakeHitEffect();
            if (--_remainHitCount <= 0)
            {
                PoolManager.Instance.BulletPoolFactory.Release(gameObject);
            }
        }
        else if (other.CompareTag("Wall"))
        {
            IBounceable bounceable = GetComponent<IBounceable>();
            if (bounceable != null)
            {
                bounceable.Bounce(other);
            }
            else
            {
                PoolManager.Instance.BulletPoolFactory.Release(gameObject);
            }
        }
    }

    private void MakeHitEffect()
    {
        if (_bulletStat != null && _bulletStat.BulletHitVfx != null)
        {
            GameObject vfx = PoolManager.Instance.VfxPoolFactory.Get(_bulletStat.BulletHitVfx, transform.position, transform.rotation);
            if (vfx != null)
            {
                EffectAutoDespawn effect = vfx.GetComponent<EffectAutoDespawn>();
                if (effect != null)
                {
                    effect.SetColor(_bulletColor.GetBulletColor);
                }
            }
        }
    }
}
