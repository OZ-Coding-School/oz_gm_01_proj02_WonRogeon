using System.Collections;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance;

    [Header("Current Zone")]
    [SerializeField] private Zone currentZone;

    private bool isTransitioning = false;

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
        // 필수 싱글톤 준비 대기
        while (FadeController.Instance == null)
            yield return null;

        while (PoolManager.Instance == null)
            yield return null;

        while (Player.Instance == null)
            yield return null;

        while (currentZone == null)
            yield return null;

        // 풀 리셋
        PoolManager.Instance.ResetPool("StartScene_Light");

        // 페이드 인
        yield return FadeController.Instance.FadeIn();

        // 최초 Zone 진입
        currentZone.OnEnter();
    }
}
