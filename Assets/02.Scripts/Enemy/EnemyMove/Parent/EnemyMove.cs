using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    protected Enemy _enemy;
    [Header("기본 이동속도")][SerializeField]protected float _basicMoveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected abstract void Move();
}
