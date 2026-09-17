using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public int BulletCount;
    //각 기준은 배율을 가리킵니다
    public float AttackDamage;
    public float AttackSpeed;
    public float MoveSpeed;
    public float ExpGetMultiplier;
    public GameObject PlayerDieVfx;
}
