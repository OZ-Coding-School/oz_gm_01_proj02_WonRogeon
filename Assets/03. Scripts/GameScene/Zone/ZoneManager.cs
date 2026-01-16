using System.Collections;
using UnityEngine;

/// <summary>
/// GameScene 내 Zone 전환을 관리하는 매니저
/// </summary>
public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance;

    [Header("Current Zone")]
    [SerializeField] private Zone currentZone;

    // 중복 코루틴 방지, 전환 안정성 확보 등을 위한 불변수
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(SceneStartRoutine());
    }

    public void ChangeZone(Zone nextZone, ZoneSpawnPoint spawnPoint)
    {
        if (isTransitioning || nextZone == null)
            return;

        StartCoroutine(ChangeZoneRoutine(nextZone, spawnPoint));
    }


    // Fade Out/In, 이전 Zone비활성화 다음 Zone 활성화, 플레이어 위치 이동, Zone 규칙적용
    // 등은 한 프레임에 끝나면 안되는 작업이기에 코루틴으로 설계
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

    // 페이드인이 끝나고 Zone.Enter()가 호출되도록 코루틴 처리
    private IEnumerator SceneStartRoutine()
    {
        // 페이드 인 완료까지 대기
        yield return FadeController.Instance.FadeIn();

        // 페이드가 끝난 뒤에 Zone 진입 처리
        if (currentZone != null)
            currentZone.OnEnter();
    }

}
