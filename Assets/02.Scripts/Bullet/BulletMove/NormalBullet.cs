using UnityEngine;

public class NormalBullet : BulletMove
{
 

    protected override void Move()
    {
        transform.Translate(Vector3.up * (_bullet.BulletStat.BulletSpeed * Time.deltaTime));
    }
}
