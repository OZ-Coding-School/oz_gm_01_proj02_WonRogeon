using UnityEngine;

/// <summary>
/// Zone 규칙에 따라 카메라 동작을 제어
/// </summary>
public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private Transform followTarget;   // 따라갈 대상
    [SerializeField] private float followSpeed = 5f;

    private bool isFollowing = false;
    private Vector3 fixedPosition;
    // isFollowing이 true면 플레이어 추적모드
    // false면 고정 위치 모드
    // FSM은 아니지만 상태 기반 설계임!

    [SerializeField] private float cameraZ = -10f;
    // 카메라의 Z값은 초기값인 -10으로 고정

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (isFollowing && followTarget != null)
        {
            Vector3 desired = followTarget.position;
            desired.z = cameraZ;

            transform.position = Vector3.MoveTowards(
                                 transform.position,
                                 desired,
                                 followSpeed * Time.deltaTime);

            // 부드러운 감속이 목적이면 Lerp,
            // 안정적인 추적이 목적이면 MoveTowards가 더 적합
        }
        else
        {
            transform.position = new Vector3(
                fixedPosition.x,
                fixedPosition.y,
                cameraZ
            );
        }
    }


    // 카메라가 플레이어가 아닌 해당 Zone의 카메라포인트에 있을 때 
    public void SetFixed(Vector3 position)
    {
        isFollowing = false;
        fixedPosition = new Vector3(position.x, position.y, cameraZ);
        transform.position = fixedPosition;
    }

    // 카메라가 플레이어 추적상태일때 (보통 넓은 방에서 사용할 예정)
    public void SetFollow(Transform target)
    {
        followTarget = target;
        isFollowing = true;
    }
}
