using UnityEngine;

public enum BulletType
{
    normal,
}
public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _lifeTime = 5f;
    private float _remainingLifeTime;

    private void Update()
    {
        transform.Translate(Vector3.up * (_speed * Time.deltaTime));
        _remainingLifeTime -= Time.deltaTime;

        if (_remainingLifeTime <= 0f)
        {
            PoolManager.Instance.BulletPoolFactory.Release(this);
        }
    }

    public void Spawn()
    {
        _remainingLifeTime = _lifeTime;
    }

    public void Despawn()
    {
        _remainingLifeTime = 0f;
    }
}
