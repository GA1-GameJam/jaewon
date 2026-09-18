using SimpleInputNamespace;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("플레이어 기본 움직임 값")][SerializeField] private float _basicMoveSpeed=6f;
    [SerializeField] private float _acceleration = 20f;
    [SerializeField] private float _deceleration = 10f;
    [Header("이동 조이스틱")]
    [SerializeField] private Joystick _moveJoystick;

    private Vector2 _currentVelocity;

    private void Start()
    {
        if (_moveJoystick == null)
        {
            GameObject joystickObject = GameObject.Find("MoveJoystick");
            if (joystickObject == null)
            {
                joystickObject = GameObject.Find("moveJoystick");
            }

            if (joystickObject != null)
            {
                _moveJoystick = joystickObject.GetComponent<Joystick>();
            }
        }
    }
    

    void Update()
    {
        PlayerMovementCheck();
    }

    private void PlayerMovementCheck()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.A)) h -= 1f;
        if (Input.GetKey(KeyCode.D)) h += 1f;
        if (Input.GetKey(KeyCode.S)) v -= 1f;
        if (Input.GetKey(KeyCode.W)) v += 1f;

        Vector2 keyboardInput = new Vector2(h, v);
        Vector2 joystickInput = _moveJoystick != null ? _moveJoystick.Value : Vector2.zero;
        Vector2 inputDirection = Vector2.ClampMagnitude(keyboardInput + joystickInput, 1f);

        if (inputDirection != Vector2.zero)
        {
            _currentVelocity = Vector2.MoveTowards(
                _currentVelocity,
                inputDirection * GameManager.Instance.Player.PlayerStat.MoveSpeed*_basicMoveSpeed,
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
