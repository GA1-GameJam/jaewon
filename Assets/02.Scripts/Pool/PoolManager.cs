using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager Instance => _instance;
    [Header("총알 팩토리")]
    [SerializeField] private PoolFactory _bulletPoolFactory;
    public PoolFactory BulletPoolFactory => _bulletPoolFactory;

    [Header("적 팩토리")]
    [SerializeField] private PoolFactory _enemyPoolFactory;
    public PoolFactory EnemyPoolFactory => _enemyPoolFactory;

    [Header("이펙트 팩토리")]
    [SerializeField] private PoolFactory _vfxPoolFactory;
    public PoolFactory VfxPoolFactory => _vfxPoolFactory;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearUnusedPools()
    {
        _bulletPoolFactory?.ClearUnusedPools();
        _enemyPoolFactory?.ClearUnusedPools();
        _vfxPoolFactory?.ClearUnusedPools();
    }
}
