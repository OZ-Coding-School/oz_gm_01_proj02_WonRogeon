using UnityEngine;

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

        if (enemy.sensor.IsPlayerDetected())
        {
            enemy.fsm.ChangeState(new ChaseState(enemy));
        }
    }

    public void Exit()
    {
        enemy.movement.Stop();
    }
}
