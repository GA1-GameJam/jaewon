using UnityEngine;

public class Blade : MonoBehaviour
{
    [Header("기본 공격력")]
    [SerializeField] private float _basicDamage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_basicDamage*GameManager.Instance.Player.PlayerStat.AttackDamage);
        }
    }

}
