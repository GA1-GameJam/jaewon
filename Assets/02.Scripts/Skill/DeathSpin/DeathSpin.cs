using System;
using UnityEngine;

public class Deathspin : MonoBehaviour
{
    [Header("생존시간")][SerializeField]private float _lifeTime=2f;
    [Header("데미지")][SerializeField]private float _damage=0.5f;
    [Header("파괴VFx")][SerializeField]private GameObject _destroyVfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<ObjectRotator>().StartRotate();
        GetComponent<ObjectViber>().StartVibe();
        GetComponent<ObjectScaler>().SizeUpStart();
    }

    // Update is called once per frame
    void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
      
            GameObject vfx = PoolManager.Instance.VfxPoolFactory.Get(_destroyVfx, transform.position, transform.rotation);
            if (vfx != null)
            {
                EffectAutoDespawn effect = vfx.GetComponent<EffectAutoDespawn>();
                if (effect != null)
                {
                    //effect.SetColor(_bulletColor.GetBulletColor);
                }
            }
        
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_damage*GameManager.Instance.Player.PlayerStat.AttackDamage);
        }
    }
}
