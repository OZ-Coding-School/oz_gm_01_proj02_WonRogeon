using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 2f;

    [Header("Detection")]
    public float viewDistance = 5f;
    public float viewAngle = 60f;
    public float ChaseDistance = 5f;      // 추격 시작
    public float ChaseLoseDistance = 7f;  // 추격 포기

    [HideInInspector]
    public Vector2 facingDir = Vector2.down; // 기본 정면
    public Animator animator;

    public Transform player;

    [HideInInspector] public Vector2 spawnPosition;

    // FSM은 적의 행동 흐름을 관리하는 객체이고
    // 그 행동의 주체는 Enemy이기 때문에 FSM은 Enemy에 소속
    public EnemyFSM fsm;
    public EnemyMovement movement;
    public EnemySensor sensor;

    // 매번 new상태를 만드는건 성능적 측면에서 별로라
    // 미리 필드를 만들고 Awake에서 생성하는걸로 수정
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public ReturnState returnState;

    // 이전 상태 캐싱을 위한 필드 추가
    private Vector2 prevFacingDir;
    private bool wasMoving;

    // A*알고리즘을 위한 벽테두리 레이어마스크 지정
    [SerializeField] private LayerMask obstacleMask;
    public LayerMask ObstacleMask => obstacleMask;


    private void Awake()
    {
        spawnPosition = transform.position;

        animator = GetComponent<Animator>();

        movement = new EnemyMovement(this, GetComponent<Rigidbody2D>(), moveSpeed);
        sensor = new EnemySensor(this, player, viewDistance, viewAngle);


        fsm = new EnemyFSM();

        // 상태 객체는 한 번만 생성
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        returnState = new ReturnState(this);

        // 초기 상태 설정
        fsm.ChangeState(patrolState);

    }

    private void Update()
    {
        fsm.Update();
        UpdateAnimation();
        //폴링은 괜찮지만 사이드 이펙트는 최소화해야하기에 UpdateAnimation 수정
    }

    public void UpdateAnimation()
    {
        bool isMoving = facingDir != Vector2.zero;

        // 이동 상태 변경 시에만 speed 갱신
        if (isMoving != wasMoving)
        {
            animator.speed = isMoving ? 1f : 0f;
            wasMoving = isMoving;
        }

        // 방향이 바뀌었을 때만 파라미터 갱신
        if (isMoving && facingDir != prevFacingDir)
        {
            animator.SetFloat("MoveX", facingDir.x);
            animator.SetFloat("MoveY", facingDir.y);
            prevFacingDir = facingDir;
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
