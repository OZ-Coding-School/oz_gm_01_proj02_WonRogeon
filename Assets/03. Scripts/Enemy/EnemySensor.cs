using UnityEngine;

// 에너미 센서는 판단 전용 객체
// enemy는 플레이어를 봤는지 직접 계산하지 않고
// Sensor에게 물어보는 형태
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

        // sqrt 연산 제거한 버전!
        float sqrDistance = toPlayer.sqrMagnitude;
        float sqrViewDistance = distance * distance;

        if (sqrDistance > sqrViewDistance)
            return false;


        float dot = Vector2.Dot(enemy.facingDir, toPlayer.normalized);
        float viewCos = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);

        return dot >= viewCos;
    }

    public float SqrDistanceToPlayer()
    {
        return (enemy.transform.position - player.position).sqrMagnitude;
    }
}

