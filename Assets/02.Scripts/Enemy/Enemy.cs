using UnityEngine;


public enum EnemyType
{
    Normal,
    
}
public class Enemy : MonoBehaviour
{
    private EnemyStat _enemyStat;

    public EnemyStat EnemyStat => _enemyStat;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
