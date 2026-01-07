using UnityEngine;

// 추격 상태
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
        enemy.movement.Move(dir);    //현재 단순 이동으로 구현했지만 나중에 A*알고리즘으로 전환

        // 추격 포기 조건
        float sqrDist = enemy.sensor.SqrDistanceToPlayer();
        float sqrChaseDist = enemy.chaseDistance * enemy.chaseDistance;

        if (sqrDist > sqrChaseDist)
        {
            enemy.fsm.ChangeState(enemy.returnState);
        }
    }

    public void Exit()
    {
        enemy.movement.Stop();
    }
}
