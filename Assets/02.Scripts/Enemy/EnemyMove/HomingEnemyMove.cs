using UnityEngine;

public class HomingEnemyMove : EnemyMove
{
    

    
    protected override void Move()
    {
        Vector3 direction = GameManager.Instance.Player.transform.position - transform.position;
        transform.Translate(
            direction * 
            (_enemy.EnemyStat.MoveSpeed * _basicMoveSpeed * Time.deltaTime),
            Space.Self
        );
        
    }
    
    
}
