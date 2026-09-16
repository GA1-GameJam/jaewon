using UnityEngine;

public class BounceBullet : BulletMove, IBounceable
{
    private Vector2 _direction;
    private bool _directionInitialized;
    [Header("바운딩 횟수")][SerializeField]private int _bounceCount;

    private void OnEnable()
    {
        _directionInitialized = false;
    }

    protected override void Move()
    {
        if (_bullet == null || _bullet.BulletStat == null)
        {
            return;
        }

        if (!_directionInitialized)
        {
            _direction = transform.up;
            _directionInitialized = true;
        }

        float speed = _bullet.BulletStat.BulletSpeed * GameManager.Instance.Player.PlayerStat.MoveSpeed;
        transform.position += (Vector3)(_direction * (speed * Time.deltaTime));
        transform.up = _direction;
    }


    

    public void Bounce(Collider2D wall)
    {
        if (--_bounceCount <= 0)
        {
            PoolManager.Instance.BulletPoolFactory.Release(gameObject);
        }
        else
        {
            Vector2 normal = (Vector2)transform.position - wall.ClosestPoint(transform.position);

            if (normal.sqrMagnitude <= Mathf.Epsilon)
            {
                normal = -_direction;
            }

            _direction = Vector2.Reflect(_direction, normal.normalized).normalized;
            _directionInitialized = true;
            transform.up = _direction;

            // 충돌 직후 같은 트리거를 다시 받지 않도록 벽 바깥 방향으로 조금 밀어낸다.
            transform.position += (Vector3)(_direction * 0.01f);
        }
        
       
    }
}
