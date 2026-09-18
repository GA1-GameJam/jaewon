using UnityEngine;

public class HomingEnemyMove : EnemyMove
{
    

    
    protected override void Move()
    {
        Vector3 direction;
        if (GameManager.Instance.Player != null)
        {
             direction = GameManager.Instance.Player.transform.position - transform.position;

        }
        else
        {
            direction = Vector3.up;

        }
        transform.Translate(
            direction * 
            (_enemy.EnemyStat.MoveSpeed * _basicMoveSpeed * Time.deltaTime),
            Space.Self
        );
        
    }
    
    
}
