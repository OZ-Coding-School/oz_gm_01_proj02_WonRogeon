using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 2f;

    [Header("Detection")]
    public float viewDistance = 5f;
    public float viewAngle = 60f;
    public float chaseDistance = 5f;

    [HideInInspector]
    public Vector2 facingDir = Vector2.down; // 기본 정면
    public Animator animator;

    public Transform player;

    [HideInInspector] public Vector2 spawnPosition;

    public EnemyFSM fsm;
    public EnemyMovement movement;
    public EnemySensor sensor;

    private void Awake()
    {
        spawnPosition = transform.position;

        animator = GetComponent<Animator>();

        movement = new EnemyMovement(this, GetComponent<Rigidbody2D>(), moveSpeed);
        sensor = new EnemySensor(this, player, viewDistance, viewAngle);


        fsm = new EnemyFSM();
        fsm.ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        fsm.Update();
        UpdateAnimation();
    }

    public void UpdateAnimation()
    {
        if (facingDir != Vector2.zero)
        {
            animator.speed = 1f;
            animator.SetFloat("MoveX", facingDir.x);
            animator.SetFloat("MoveY", facingDir.y);
        }
        else
        {
            animator.speed = 0f;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (player == null)
            return;

        // 시야 색상
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position;
        float halfAngle = viewAngle * 0.5f;

        // 좌우 시야 경계선
        Vector3 leftDir = DirFromAngle(-halfAngle);
        Vector3 rightDir = DirFromAngle(halfAngle);

        // 시야 경계선 그리기
        Gizmos.DrawRay(origin, leftDir * viewDistance);
        Gizmos.DrawRay(origin, rightDir * viewDistance);

        // 거리 원
        Gizmos.DrawWireSphere(origin, viewDistance);

        // 플레이어가 감지 중이면 빨간 선
        if (sensor != null && sensor.IsPlayerDetected())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, player.position);
        }
    }

    /// <summary>
    /// 적의 forward(enemy.up) 기준 각도 계산
    /// </summary>
    private Vector3 DirFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;

        Vector3 forward = facingDir;

        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector3(
            forward.x * cos - forward.y * sin,
            forward.x * sin + forward.y * cos,
            0f
        );
    }

}
