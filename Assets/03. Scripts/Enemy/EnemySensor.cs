using UnityEngine;

public class EnemySensor
{
    private Enemy enemy;
    private Transform player;
    private float distance;
    private float angle;

    public EnemySensor(Enemy enemy, Transform player, float distance, float angle)
    {
        this.enemy = enemy;
        this.player = player;
        this.distance = distance;
        this.angle = angle;
    }

    public bool IsPlayerDetected()
    {
        Vector2 toPlayer = player.position - enemy.transform.position;

        if (toPlayer.magnitude > distance)
            return false;

        float dot = Vector2.Dot(enemy.facingDir, toPlayer.normalized);
        float viewCos = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);

        return dot >= viewCos;
    }

    public float DistanceToPlayer()
    {
        return Vector2.Distance(enemy.transform.position, player.position);
    }
}

