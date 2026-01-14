using UnityEngine;

/// <summary>
/// 문 앞 근접 시
/// 해당 문을 통해 이동하게 될 Zone 이름을
/// 미리 보여주는 트리거
/// </summary>
public class ZonePreviewTrigger : MonoBehaviour
{
    [SerializeField] private Zone targetZone;
    [SerializeField] private Transform doorTransform; // 문 위치

    [Header("Preview Text")]
    [SerializeField] private string lockedPreviewText = "???";
    [SerializeField] private string overrideZoneName;

    [Header("Door State")]
    [SerializeField] private DoorStateProvider doorState;

    private bool isPlayerInside = false;

    private void OnEnable()
    {
        if (doorState != null)
            doorState.OnDoorUnlocked += RefreshPreview;
    }

    private void OnDisable()
    {
        if (doorState != null)
            doorState.OnDoorUnlocked -= RefreshPreview;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInside = true;
        ShowPreview();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (ZoneNamePreviewUI.Instance == null)
            return;

        isPlayerInside = false;
        ZoneNamePreviewUI.Instance.Hide();
    }

    /// <summary>
    /// 문 상태가 변경되었을 때 호출됨
    /// (플레이어가 이미 범위 안에 있으면 즉시 갱신)
    /// </summary>
    private void RefreshPreview()
    {
        if (!isPlayerInside)
            return;

        ShowPreview();
    }

    private void ShowPreview()
    {
        string nameToShow;

        if (doorState != null && doorState.IsLocked)
        {
            nameToShow = lockedPreviewText;
        }
        else
        {
            nameToShow =
                string.IsNullOrEmpty(overrideZoneName)
                ? targetZone.zoneName
                : overrideZoneName;
        }

        ZoneNamePreviewUI.Instance.Show(nameToShow, doorTransform);
    }
}
