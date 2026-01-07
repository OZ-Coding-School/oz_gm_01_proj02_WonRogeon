using UnityEngine;

public class ReturnState : IEnemyState
{
    private Enemy enemy;

    public ReturnState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() { }

    public void Update()
    {
        Vector2 toSpawn =
            enemy.spawnPosition - (Vector2)enemy.transform.position;

        if (toSpawn.magnitude < 0.2f)
        {
            enemy.fsm.ChangeState(enemy.patrolState);
            return;
        }

        enemy.movement.Move(toSpawn.normalized);
    }

    public void Exit()
    {
        enemy.movement.Stop();
    }
}
