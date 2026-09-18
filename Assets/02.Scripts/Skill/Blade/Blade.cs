using UnityEngine;

public class Blade : MonoBehaviour
{
    [Header("기본 공격력")]
    [SerializeField] private float _basicDamage;
    
    [Header("공격시 이펙트")]
    [SerializeField] private GameObject _hitEffect;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_basicDamage*GameManager.Instance.Player.PlayerStat.AttackDamage);
            
                GameObject vfx = PoolManager.Instance.VfxPoolFactory.Get(_hitEffect, transform.position, transform.rotation);
                if (vfx != null)
                {
                    EffectAutoDespawn effect = vfx.GetComponent<EffectAutoDespawn>();
                    if (effect != null)
                    {
                        effect.SetColor(GetComponent<SkillColor>().BladeColor);
                    }
                }
            
        }
    }

}
