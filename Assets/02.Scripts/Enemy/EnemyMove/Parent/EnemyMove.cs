using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    private Enemy _enemy;
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
