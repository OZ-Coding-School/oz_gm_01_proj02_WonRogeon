using UnityEngine;

// 주어진 방향으로 실제 물리이동을 수행하는 컴포넌트
// 나중에 A*알고리즘 적용할 예정, 대략 1월8~9일?
public class EnemyMovement
{
    private Rigidbody2D rb;
    private float speed;
    private Enemy owner;

    public EnemyMovement(Enemy owner, Rigidbody2D rb, float speed)
    {
        this.owner = owner;
        this.rb = rb;
        this.speed = speed;

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    public void Move(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            dir = dir.normalized;
            rb.velocity = dir * speed;
            owner.facingDir = dir;   // 정면 방향 갱신
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }
}
