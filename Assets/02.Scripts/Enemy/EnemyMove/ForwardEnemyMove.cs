using UnityEngine;

public class ForwardEnemyMove : EnemyMove
{
    

    protected override void Move()
    {
        transform.Translate(
            Vector3.up * 
            (_enemy.EnemyStat.MoveSpeed * _basicMoveSpeed * Time.deltaTime),
            Space.Self
        );    }
}
