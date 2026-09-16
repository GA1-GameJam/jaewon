using System;
using UnityEngine;

public enum BulletType
{
    normal,
}
public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField]private BulletStat _bulletStat;
    public BulletStat BulletStat=>_bulletStat;
    

    public void Spawn()
    {
    }

    public void Despawn()
    {
        PoolManager.Instance.VfxPoolFactory.Get(_bulletStat.BulletDestroyVfx,transform.position,transform.rotation);
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
