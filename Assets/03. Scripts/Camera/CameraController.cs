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

    [SerializeField] private float cameraZ = -10f;

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

            transform.position = Vector3.Lerp(
                transform.position,
                desired,
                followSpeed * Time.deltaTime
            );
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


    // 카메라 고정
    public void SetFixed(Vector3 position)
    {
        isFollowing = false;
        fixedPosition = new Vector3(position.x, position.y, transform.position.z);
        transform.position = fixedPosition;
    }

    // 카메라 추적
    public void SetFollow(Transform target)
    {
        followTarget = target;
        isFollowing = true;
    }
}
