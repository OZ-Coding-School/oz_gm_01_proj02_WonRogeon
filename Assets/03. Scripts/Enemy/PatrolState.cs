using UnityEngine;

// 단순 이동하는 패트롤(순찰) 상태, 감지되면 추격으로 전환
public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private Vector2 dir;
    private float switchTimer;

    public PatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        switchTimer = 0f;
        dir = GetInitialDirection();
    }


    public void Update()
    {
        switchTimer += Time.deltaTime;

        if (switchTimer >= enemy.PatrolSwitchTime)
        {
            dir = GetNextDirection();
            switchTimer = 0f;
        }

        enemy.movement.Move(dir);

        // 만일 플레이어를 감지했다면
        if (enemy.sensor.IsPlayerDetected())
        {
            enemy.fsm.ChangeState(enemy.chaseState);
        }
    }

    // 방향 결정 메서드들
    private Vector2 GetInitialDirection()
    {
        return enemy.PatrolMoveType switch
        {
            PatrolMoveType.Vertical => Vector2.up,
            PatrolMoveType.Horizontal => Vector2.right,
            PatrolMoveType.FourDirection => GetRandomFourDir(),
            PatrolMoveType.Random => GetRandomDir(),
            _ => Vector2.up
        };
    }

    private Vector2 GetNextDirection()
    {
        return enemy.PatrolMoveType switch
        {
            PatrolMoveType.Vertical => -dir,
            PatrolMoveType.Horizontal => -dir,
            PatrolMoveType.FourDirection => GetRandomFourDir(),
            PatrolMoveType.Random => GetRandomDir(),
            _ => -dir
        };
    }

    // 보조 메서드
    private Vector2 GetRandomFourDir()
    {
        Vector2[] dirs =
        {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

        return dirs[Random.Range(0, dirs.Length)];
    }

    private Vector2 GetRandomDir()
    {
        Vector2 dir = Random.insideUnitCircle;
        return dir.normalized;
    }



    public void Exit()
    {
        enemy.movement.Stop();
    }
}
