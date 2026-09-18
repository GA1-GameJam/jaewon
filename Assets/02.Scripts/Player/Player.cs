using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private PlayerStat _playerStat;
    public PlayerStat PlayerStat => _playerStat;
    
    


    public void MoveSpeedUp(float input)
    {
        if (input < 0) return;
        _playerStat.MoveSpeed += input;
    }
    public void AttackSpeedUp(float input)
    {
        if (input < 0) return;
        _playerStat.AttackSpeed-= input;
    }
    public void AttackDamageUp(float input)
    {
        if (input < 0) return;
        _playerStat.AttackDamage+= input;
    }

    public void ExpMutliplierUp(float input)
    {
        if (input < 0) return;
        _playerStat.ExpGetMultiplier += input;
    }
    
    public void BulletCountUp(int input)
    {
        if (input < 0) return;
        _playerStat.BulletCount += input;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Instantiate(_playerStat.PlayerDieVfx,transform.position,Quaternion.identity);
            GameManager.Instance.GameOver();
            Destroy(gameObject);

        }
        else if(other.CompareTag("Wall"))
        {
            Instantiate(_playerStat.PlayerDieVfx,transform.position,Quaternion.identity);
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }
}
