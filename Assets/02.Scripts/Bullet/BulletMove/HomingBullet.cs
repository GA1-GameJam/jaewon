using UnityEngine;

public class HomingBullet : BulletMove
{
    [Header("감지 거리")]
    [SerializeField] private float _detectedRadius = 20f;

    private Enemy _target;

    private void OnEnable()
    {
        _target = null;
    }

    protected override void Move()
    {
        if (_target == null)
        {
            DetectEnemy();
            MoveForward();
        }
        else
        {
            MoveToTarget();

        }

    }

    private void DetectEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            _detectedRadius);

        float closestDistance = float.PositiveInfinity;

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy == null || !enemy.CompareTag("Enemy"))
            {
                continue;
            }

            float distance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _target = enemy;
            }
        }
    }

    private void MoveForward()
    {
        MoveInDirection(transform.up);
    }

    private void MoveToTarget()
    {
        if (!_target.gameObject.activeInHierarchy)
        {
            _target = null;
            return;
        }

        Vector3 direction = (_target.transform.position - transform.position).normalized;
        transform.up = direction;
        MoveInDirection(direction);
    }

    private void MoveInDirection(Vector3 direction)
    {
        float speed = _bullet.BulletStat.BulletSpeed * GameManager.Instance.Player.PlayerStat.MoveSpeed;
        transform.position += direction * (speed * Time.deltaTime);
    }
}
