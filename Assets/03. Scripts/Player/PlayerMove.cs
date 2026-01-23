using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    [Header("Footstep SFX")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepCooldown = 0.4f;

    private Animator animator;
    private Rigidbody2D rb;

    private Vector2 inputDir;

    private bool isMoving;
    private float footstepCooldownTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            inputDir = Vector2.zero;
            animator.speed = 0f;
            isMoving = false;
            footstepCooldownTimer = 0f;
            return;
        }

        ReadInput();
        UpdateAnimation();
        HandleFootstepCooldown();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = inputDir * moveSpeed;
    }

    private void ReadInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        inputDir = new Vector2(x, y);

        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();
    }

    private void UpdateAnimation()
    {
        if (inputDir != Vector2.zero)
        {
            animator.speed = 1f;
            animator.SetFloat("MoveX", inputDir.x);
            animator.SetFloat("MoveY", inputDir.y);

            // 이동 시작 순간
            if (!isMoving)
            {
                isMoving = true;
                footstepCooldownTimer = footstepCooldown;
            }
        }
        else
        {
            animator.speed = 0f;
            isMoving = false;
            footstepCooldownTimer = 0f;
        }
    }

    private void HandleFootstepCooldown()
    {
        if (!isMoving || footstepClip == null)
            return;

        footstepCooldownTimer += Time.deltaTime;

        if (footstepCooldownTimer >= footstepCooldown)
        {
            footstepCooldownTimer = 0f;

            if (SoundManager.Instance != null)
            {
                // Game SFX 경로로 발걸음 재생
                SoundManager.Instance.PlayGameSFXAt(transform.position, footstepClip);
            }
        }
    }
}
