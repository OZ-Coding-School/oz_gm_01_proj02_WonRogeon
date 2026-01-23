using System.Collections.Generic;
using UnityEngine;

// 추격 상태
public class ChaseState : IEnemyState
{
    private Enemy enemy;

    private List<Vector2> path;
    private int pathIndex;
    private float pathTimer;

    private float loseTimer;
    private const float loseDelay = 1.0f;

    // ===== 추격 진입 사운드 1회용 =====
    private bool playedChaseSFX;

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

        // ===== 추격 시작 사운드 =====
        if (!playedChaseSFX &&
            SoundManager.Instance != null &&
            enemy.ChaseStartSFX != null)
        {
            SoundManager.Instance.PlayGameSFXAt(
                enemy.transform.position,
                enemy.ChaseStartSFX
            );

            playedChaseSFX = true;
        }
    }

    public void Update()
    {
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
            loseTimer = 0f;
        }

        pathTimer -= Time.deltaTime;

        if (path == null && pathTimer <= 0f)
        {
            path = AStarPathfinder2D.FindPath(
                enemy.transform.position,
                enemy.player.transform.position,
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

        if (path == null || pathIndex >= path.Count)
        {
            enemy.movement.Stop();
            path = null;
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

        // 다음 추격을 위해 리셋
        playedChaseSFX = false;
    }
}

