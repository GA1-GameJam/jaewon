using UnityEngine;

public class PlayerFire : MonoBehaviour
{

    [Header("총알 발사 지점")]
    [SerializeField] private Transform _bulletPoint;
    [Header("총알 간격")]
    [SerializeField] private float _bulletSpacing=0.5f;
    private Player _player;
    private float _curTime;
    
    [SerializeField] private GameObject _curEquipedBulletPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
    void Start()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _curTime-=Time.deltaTime;
        if (_curTime <= 0)
        {
            FireBullet();
        }
    }


    private void FireBullet()
    {
        _curTime = _player.PlayerStat.AttackSpeed;
        

        for (int i = 0; i < _player.PlayerStat.BulletCount; i++)
        {
            float offset = (i - (_player.PlayerStat.BulletCount - 1) / 2f) * _bulletSpacing;

            Vector3 bulletPos =
                _bulletPoint.position +
                transform.right * offset;

            PoolManager.Instance.BulletPoolFactory.Get(
                _curEquipedBulletPrefab,
                bulletPos,
                transform.rotation
            );
        }
    }
    
    
}
