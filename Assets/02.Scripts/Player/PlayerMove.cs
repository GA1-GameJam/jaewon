using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player _player;
    
    [SerializeField] private float _acceleration = 20f;
    [SerializeField] private float _deceleration = 10f;

    private Vector2 _currentVelocity;

    private void Start()
    {
        _player = GetComponent<Player>();
    }
    

    void Update()
    {
        PlayerMovementCheck();
    }

    private void PlayerMovementCheck()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 inputDirection = new Vector2(h, v).normalized;

        // 입력이 있을 때
        if (inputDirection != Vector2.zero)
        {
            // 현재 속도를 목표 방향/속도로 서서히 변경
            _currentVelocity = Vector2.MoveTowards(
                _currentVelocity,
                inputDirection * _player.PlayerStat.MoveSpeed,
                _acceleration * Time.deltaTime
            );

            float angle = Vector2.SignedAngle(
                Vector2.up,
                inputDirection
            );

            transform.rotation = Quaternion.Euler(0f, 0f, angle);

        
        }
        else
        {
            // 입력이 없으면 서서히 감속
            _currentVelocity = Vector2.MoveTowards(
                _currentVelocity,
                Vector2.zero,
                _deceleration * Time.deltaTime
            );
        }

        // 실제 이동
        transform.Translate(
            _currentVelocity * Time.deltaTime,
            Space.World
        );
    }

    
}
