using UnityEngine;
using TMPro;

/// <summary>
/// 현재 Zone의 층 / 이름 정보를
/// 왼쪽 하단 UI로 출력하는 전용 UI 컨트롤러
/// </summary>
public class ZoneInfoUI : MonoBehaviour
{
    public static ZoneInfoUI Instance;

    [Header("UI References")]
    [SerializeField] private TMP_Text floorText;
    [SerializeField] private TMP_Text zoneNameText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Zone 진입 시 호출되어
    /// 층 정보와 Zone 이름을 UI에 반영
    /// </summary>
    public void SetZoneInfo(int floor, string zoneName)
    {
        // 층 표기 (예: 1F)
        if (floorText != null)
            floorText.text = $"{floor}F";

        // Zone 이름 표기
        if (zoneNameText != null)
            zoneNameText.text = zoneName;
    }
}
