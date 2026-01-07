using UnityEngine;

/// <summary>
/// 플레이어가 닿으면 Zone을 변경하는 트리거
/// </summary>
public class ZoneTransitionTrigger : MonoBehaviour
{
    [SerializeField] private Zone targetZone;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        ZoneManager.Instance.ChangeZone(targetZone);
    }
}
