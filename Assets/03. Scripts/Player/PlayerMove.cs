using UnityEngine;

/// <summary>
/// 플레이어 이동 + 8방향 애니메이션 제어
/// Rigidbody2D.velocity 기반 이동 (충돌 / 피격 / 장애물 대응용)
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Animator animator;
    private Rigidbody2D rb;

    private Vector2 inputDir;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 직접이동하니까 회전 고정
        rb.freezeRotation = true;
    }

    private void Update()
    {
        ReadInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 입력 처리 (대각선 보정 포함)
    /// </summary>
    private void ReadInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        inputDir = new Vector2(x, y);

        // 대각선 속도 보정
        // sqrMagnitude를 쓴 이유는 magnitude 내부적으로 sqrt연산이 들어가는데
        // sqrMagnitude는 비교용으로 훨씬 가볍기 때문!
        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();
    }

    /// <summary>
    /// Rigidbody2D를 이용한 물리 이동
    /// </summary>
    private void Move()
    {
        rb.velocity = inputDir * moveSpeed;
    }

    /// <summary>
    /// Blend Tree용 애니메이션 파라미터 갱신
    /// (정지 시 마지막 방향 프레임 유지)
    /// </summary>
    private void UpdateAnimation()
    {
        if (inputDir != Vector2.zero)
        {
            animator.speed = 1f;
            animator.SetFloat("MoveX", inputDir.x);
            animator.SetFloat("MoveY", inputDir.y);
        }
        else
        {
            // 입력 없을 때 현재 프레임에서 정지
            animator.speed = 0f;
        }
    }
}
