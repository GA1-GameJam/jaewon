using System.Collections.Generic;
using SimpleInputNamespace;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Header("화구 오브젝트")]
    [SerializeField]private GameObject _fireHead;
    [Header("총알 기본 쿨타임")]
    [SerializeField] private float _basicBulletCoolTime=1f;
    [Header("총알 발사 지점")]
    [SerializeField] private Transform _bulletPoint;
    [Header("총알 간격")]
    [SerializeField] private float _bulletSpacing=0.5f;
    [Header("총알 발사 사운드")]
    [SerializeField] private AudioClip _bulletFireSound;
    [Header("조준 조이스틱")]
    [SerializeField] private Joystick _fireHeadJoystick;
    private Player _player;
    private float _curTime;

    [Header("총알 업그레이드를 위한 레벨")]
    [SerializeField] private int _levelForUpgrade;
    [Header("총알 업그레이드 후보")]
    [SerializeField] private List<GameObject> _bulletPrefLists;

    private int _bulletIdx = 0;

    [SerializeField] private GameObject _curEquipedBulletPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
    void Start()
    {
        _player = GetComponent<Player>();
        EquipBullet();
    }

    // Update is called once per frame
    void Update()
    {
        _curTime-=Time.deltaTime;
        if (_curTime <= 0)
        {
            FireBullet();
        }

        RotateFireHead();
    }

    private void RotateFireHead()
    {
        // 이동 입력(Horizontal/Vertical)과 겹치지 않도록 화살표 키만 읽는다.
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) h -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) h += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) v -= 1f;
        if (Input.GetKey(KeyCode.UpArrow)) v += 1f;

        Vector2 keyboardInput = new Vector2(h, v);
        Vector2 joystickInput = _fireHeadJoystick != null ? _fireHeadJoystick.Value : Vector2.zero;
        Vector2 inputDirection = Vector2.ClampMagnitude(keyboardInput + joystickInput, 1f);

        // 입력이 없으면 마지막 조준 방향을 유지한다.
        if (inputDirection == Vector2.zero || _fireHead == null)
        {
            return;
        }

        float angle = Vector2.SignedAngle(Vector2.up, inputDirection);
        _fireHead.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FireBullet()
    {
        CheckEnableUpgrade();
        
        AudioManager.Instance.PlayEffectClip(_bulletFireSound);
        _curTime = _basicBulletCoolTime*_player.PlayerStat.AttackSpeed;

        Transform fireTransform = _fireHead != null ? _fireHead.transform : transform;

        for (int i = 0; i < _player.PlayerStat.BulletCount; i++)
        {
            float offset = (i - (_player.PlayerStat.BulletCount - 1) / 2f) * _bulletSpacing;

            Vector3 bulletPos =
                _bulletPoint.position +
                fireTransform.right * offset;

            PoolManager.Instance.BulletPoolFactory.Get(
                _curEquipedBulletPrefab,
                bulletPos,
                fireTransform.rotation
            );
        }
    }


    public void CheckEnableUpgrade()
    {
        if (_player.PlayerStat.BulletCount >= _levelForUpgrade)
        {
            _player.PlayerStat.BulletCount = 1;
            UpgradeBullet();
        }
    }
    public void UpgradeBullet()
    {
        _bulletIdx++;
        EquipBullet();
    }

    private void EquipBullet()
    {
        _curEquipedBulletPrefab=_bulletPrefLists[_bulletIdx];

    }
    
}
