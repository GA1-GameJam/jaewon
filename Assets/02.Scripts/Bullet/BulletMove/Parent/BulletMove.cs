using UnityEngine;

public abstract class BulletMove : MonoBehaviour
{
    protected Bullet _bullet;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bullet=GetComponent<Bullet>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    protected abstract void Move();
    
}
