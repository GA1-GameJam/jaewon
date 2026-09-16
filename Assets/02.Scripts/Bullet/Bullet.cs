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
   
   private BulletColor _bulletColor; 

    private void Awake()
    {
        _bulletColor=GetComponent<BulletColor>();
    }



    public void Spawn()
    {
        _bulletColor.DecisionColorToAttackDamage();
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
                    effect.SetColor(_bulletColor.GetBulletColor);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적에게 데미지 전달
            other.GetComponent<Enemy>()?.TakeDamage(_bulletStat.BulletDamage*GameManager.Instance.Player.PlayerStat.AttackDamage);
        }

        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            PoolManager.Instance.BulletPoolFactory.Release(gameObject);
        }
    }
}
