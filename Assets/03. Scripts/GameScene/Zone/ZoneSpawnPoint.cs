using UnityEngine;

/// <summary>
/// Zone 진입 시 플레이어가 등장할 위치
/// ZoneTransitionTrigger와 1:1로 사용됨
/// </summary>
public class ZoneSpawnPoint : MonoBehaviour
{
    // 마커 용도
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.up * 0.5f
        );
    }
}
