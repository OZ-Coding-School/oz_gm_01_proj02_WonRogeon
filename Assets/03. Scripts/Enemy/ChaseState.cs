using UnityEngine;

public class ChaseState : IEnemyState
{
    private Enemy enemy;

    public ChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() { }

    public void Update()
    {
        Vector2 dir =
            (enemy.player.position - enemy.transform.position).normalized;

        enemy.facingDir = dir;
        enemy.movement.Move(dir);

        // 추격 포기 조건
        if (enemy.sensor.DistanceToPlayer() > enemy.chaseDistance)
        {
            enemy.fsm.ChangeState(new ReturnState(enemy));
        }
    }

    public void Exit()
    {
        enemy.movement.Stop();
    }
}
