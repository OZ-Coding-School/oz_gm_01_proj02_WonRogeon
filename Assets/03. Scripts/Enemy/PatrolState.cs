using UnityEngine;

// 단순 이동하는 패트롤(순찰) 상태, 감지되면 추격으로 전환
public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private Vector2 dir = Vector2.up;
    private float switchTimer;

    public PatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        switchTimer = 0;
    }

    public void Update()
    {
        switchTimer += Time.deltaTime;

        if (switchTimer > 2f)
        {
            dir = -dir;
            switchTimer = 0;
        }

        enemy.movement.Move(dir);

        // 만일 플레이어를 감지했다면
        if (enemy.sensor.IsPlayerDetected())
        {
            enemy.fsm.ChangeState(enemy.chaseState);
        }
    }

    public void Exit()
    {
        enemy.movement.Stop();
    }
}
