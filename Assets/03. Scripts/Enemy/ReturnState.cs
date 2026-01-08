using System.Collections.Generic;
using UnityEngine;

// 복귀 상태
public class ReturnState : IEnemyState
{
    private Enemy enemy;

    private List<Vector2> path;
    private int pathIndex;
    private float pathTimer;

    public ReturnState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        path = null;
        pathIndex = 0;
        pathTimer = 0f;
    }

    public void Update()
    {
        Vector2 returnTarget = enemy.spawnPosition; // 또는 patrol 기준점

        // ===== 1. A* 경로 계산 =====

        pathTimer -= Time.deltaTime;

        if (path == null && pathTimer <= 0f)
        {
            path = AStarPathfinder2D.FindPath(
                enemy.transform.position,
                returnTarget,
                1f,
                enemy.ObstacleMask
            );

            pathIndex = 0;

            if (path != null && path.Count > 0 &&
                Vector2.Distance(enemy.transform.position, path[0]) < 0.25f)
            {
                pathIndex = 1;
            }

            pathTimer = 0.4f;
        }

        // ===== 2. 도착 판정 =====

        if (path == null || pathIndex >= path.Count)
        {
            enemy.movement.Stop();
            enemy.fsm.ChangeState(enemy.patrolState);
            return;
        }

        // ===== 3. 이동 =====

        Vector2 target = path[pathIndex];
        Vector2 dir = target - (Vector2)enemy.transform.position;

        if (dir.magnitude < 0.25f)
        {
            pathIndex++;
            return;
        }

        enemy.movement.Move(dir.normalized);
    }

    public void Exit()
    {
        enemy.movement.Stop();
        path = null;
    }
}
