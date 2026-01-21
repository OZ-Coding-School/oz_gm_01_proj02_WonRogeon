using UnityEngine;

/// <summary>
/// GameScene 내부의 하나의 구역(Zone)
/// 맵 조각이 아닌 하나의 규칙 묶음이자 연출 단위
/// 씬 전환 없이도 장면을 바꾼 것처럼 느끼게 만듦
/// </summary>
public class Zone : MonoBehaviour
{
    [Header("Zone Info")]
    public string zoneName;
    public int floor;

    /// <summary>
    /// 저장/로드 및 UI 표시용 Zone 이름
    /// 예: "1F Aya Room"
    /// </summary>
    public string DisplayName
    {
        get { return $"{floor}F {zoneName}"; }
    }

    // =========================
    // Lighting Settings
    // =========================
    [Header("Lighting Settings")]
    [Range(0f, 1f)]
    [SerializeField]
    private float globalLightIntensity = 0.3f;

    public float GlobalLightIntensity => globalLightIntensity;

    // =========================
    // Camera Settings
    // =========================
    [Header("Camera Settings")]
    public bool followCamera = false;
    public Transform cameraFixedPoint;

    public virtual void OnEnter()
    {
        Debug.Log($"[Zone Enter] {zoneName}");

        // 카메라 처리
        if (followCamera)
        {
            CameraController.Instance.SetFollow(Player.Instance.transform);
        }
        else if (cameraFixedPoint != null)
        {
            CameraController.Instance.SetFixed(cameraFixedPoint.position);
        }

        // Zone UI
        ZoneInfoUI.Instance.SetZoneInfo(floor, zoneName);
    }

    public virtual void OnExit()
    {
        Debug.Log($"[Zone Exit] {zoneName}");
    }

    // virtual 가상메서드로 만든 이유:
    // 이후 이벤트 Zone, 컷신 Zone 등 확장 대비
}
