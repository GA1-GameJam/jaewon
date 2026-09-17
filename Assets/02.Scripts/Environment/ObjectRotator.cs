using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("회전 목표 속도")]
    [SerializeField] private float _rotateSpeed = 50f;
    [Header("회전 가속도")]
    [SerializeField] private float _acceleration = 100f;

    private float _currentRotateSpeed;
    private float _targetRotateSpeed;
    private bool _isRotating;

    private void Start()
    {
        StartRotate();
    }

    private void Update()
    {
        if (_isRotating)
        {
            Rotate();
        }
    }

    public void StartRotate()
    {
        _currentRotateSpeed = 0f;
        _isRotating = true;
    }

    public void StopRotate()
    {
        _isRotating = false;
        _currentRotateSpeed = 0f;
    }

    private void Rotate()
    {
        _currentRotateSpeed = Mathf.MoveTowards(
            _currentRotateSpeed,
            _rotateSpeed,
            _acceleration * Time.deltaTime);

        transform.Rotate(new Vector3(0f, 0f, _currentRotateSpeed * Time.deltaTime));
    }
}
