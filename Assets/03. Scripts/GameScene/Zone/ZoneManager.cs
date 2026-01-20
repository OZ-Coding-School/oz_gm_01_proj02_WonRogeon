using System.Collections;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance;

    [Header("Current Zone")]
    [SerializeField] private Zone currentZone;
    public Zone CurrentZone => currentZone;


    private bool isTransitioning = false;

    /// <summary>
    /// 저장/로드용 현재 Zone 표시 이름
    /// </summary>
    public string CurrentZoneDisplayName
    {
        get
        {
            return currentZone != null
                ? currentZone.DisplayName   // 예: "1F Aya Room"
                : "Unknown";
        }
    }

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

    private void Start()
    {
        StartCoroutine(SceneStartRoutineSafe());
    }

    public void ChangeZone(Zone nextZone, ZoneSpawnPoint spawnPoint)
    {
        if (isTransitioning || nextZone == null)
            return;

        StartCoroutine(ChangeZoneRoutine(nextZone, spawnPoint));
    }

    private IEnumerator ChangeZoneRoutine(
        Zone nextZone,
        ZoneSpawnPoint spawnPoint)
    {
        isTransitioning = true;

        yield return FadeController.Instance.FadeOut();

        if (currentZone != null)
            currentZone.gameObject.SetActive(false);

        nextZone.gameObject.SetActive(true);
        currentZone = nextZone;

        if (Player.Instance != null && spawnPoint != null)
        {
            Player.Instance.transform.position = spawnPoint.transform.position;
        }

        CameraController.Instance.transform.position =
            new Vector3(
                Player.Instance.transform.position.x,
                Player.Instance.transform.position.y,
                -10f
            );

        currentZone.OnEnter();

        yield return FadeController.Instance.FadeIn();

        isTransitioning = false;
    }

    // ===== 안전한 시작 루틴 =====
    private IEnumerator SceneStartRoutineSafe()
    {
        while (FadeController.Instance == null)
            yield return null;

        while (PoolManager.Instance == null)
            yield return null;

        while (Player.Instance == null)
            yield return null;

        while (currentZone == null)
            yield return null;

        PoolManager.Instance.ResetPool("StartScene_Light");

        yield return FadeController.Instance.FadeIn();

        currentZone.OnEnter();
    }

    public void LoadZone(int floor, string zoneName)
    {
        StartCoroutine(LoadZoneRoutine(floor, zoneName));
    }

    private IEnumerator LoadZoneRoutine(int floor, string zoneName)
    {
        yield return FadeController.Instance.FadeOut();

        Zone target = FindZone(floor, zoneName);
        if (target == null)
        {
            Debug.LogError($"[Load] Zone not found: {floor}F {zoneName}");
            yield break;
        }

        if (currentZone != null)
            currentZone.gameObject.SetActive(false);

        target.gameObject.SetActive(true);
        currentZone = target;

        Player.Instance.transform.position = target.transform.position;

        CameraController.Instance.transform.position =
            new Vector3(
                Player.Instance.transform.position.x,
                Player.Instance.transform.position.y,
                -10f
            );

        currentZone.OnEnter();

        yield return FadeController.Instance.FadeIn();
    }

    private Zone FindZone(int floor, string zoneName)
    {
        Zone[] zones = FindObjectsOfType<Zone>(true); // 비활성 포함
        foreach (var z in zones)
        {
            if (z.floor == floor && z.zoneName == zoneName)
                return z;
        }
        return null;
    }

}
