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
    
    
    
    
}
