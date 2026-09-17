using UnityEngine;

[System.Serializable]
public class BulletStat 
{ 
    public float BulletSpeed;
    public float BulletDamage;
    public int HitEnableCount;
    public GameObject BulletHitVfx;
    public GameObject BulletDestroyVfx;
    public AudioClip BulletHitSound;
    public AudioClip BulletDestroySound;
}
