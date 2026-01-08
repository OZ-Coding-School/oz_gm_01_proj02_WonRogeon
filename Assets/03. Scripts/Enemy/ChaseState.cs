using System.Collections.Generic;
using UnityEngine;

// 추격 상태
public class ChaseState : IEnemyState
{
    private Enemy enemy;

    private List<Vector2> path;
    private int pathIndex;
    private float pathTimer;

    // 추격 포기 지연용
    private float loseTimer;
    private const float loseDelay = 1.0f; // 1초 유지되면 포기

    public ChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        path = null;
        pathIndex = 0;
        pathTimer = 0f;
        loseTimer = 0f;
    }

    public void Update()
    {
        // ===== 1. 추격 포기 판정 (지연 포함) =====

        float distance = Vector2.Distance(
            enemy.transform.position,
            enemy.player.transform.position
        );

        if (distance > enemy.ChaseLoseDistance)
        {
            loseTimer += Time.deltaTime;

            if (loseTimer >= loseDelay)
            {
                enemy.fsm.ChangeState(enemy.returnState);
                return;
            }
        }
        else
        {
            // 다시 가까워지면 포기 타이머 리셋
            loseTimer = 0f;
        }

        // ===== 2. A* 경로 계산 =====

        pathTimer -= Time.deltaTime;

        if (path == null && pathTimer <= 0f)
        {
            path = AStarPathfinder2D.FindPath(
                enemy.transform.position,
                enemy.player.transform.position,
                1f,                // 타일 크기
                enemy.ObstacleMask
            );

            pathIndex = 0;

            // 시작 노드가 자기 위치면 스킵
            if (path != null && path.Count > 0 &&
                Vector2.Distance(enemy.transform.position, path[0]) < 0.25f)
            {
                pathIndex = 1;
            }

            pathTimer = 0.4f;
        }

        // ===== 3. 이동 =====

        if (path == null || pathIndex >= path.Count)
        {
            enemy.movement.Stop();
            path = null; // 다음 프레임에만 재계산
            return;
        }

        Vector2 target = path[pathIndex];
        Vector2 toTarget = target - (Vector2)enemy.transform.position;

        if (toTarget.magnitude < 0.25f)
        {
            pathIndex++;
            return;
        }

        enemy.movement.Move(toTarget.normalized);
    }

    public void Exit()
    {
        enemy.movement.Stop();
        path = null;
        loseTimer = 0f;
    }
}
