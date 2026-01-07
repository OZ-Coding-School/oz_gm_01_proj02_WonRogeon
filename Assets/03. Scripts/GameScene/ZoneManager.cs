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

    public void ChangeZone(Zone nextZone)
    {
        if (isTransitioning || nextZone == null)
            return;

        StartCoroutine(ChangeZoneRoutine(nextZone));
    }

    // Fade Out/In, 이전 Zone비활성화 다음 Zone 활성화, 플레이어 위치 이동, Zone 규칙적용
    // 등은 한 프레임에 끝나면 안되는 작업이기에 코루틴으로 설계
    private IEnumerator ChangeZoneRoutine(Zone nextZone)
    {
        isTransitioning = true;

        // 화면 어두워짐
        yield return FadeController.Instance.FadeOut();

        // 이전 존 비활성화
        if (currentZone != null)
            currentZone.gameObject.SetActive(false);

        // 다음 존 활성화
        nextZone.gameObject.SetActive(true);
        currentZone = nextZone;

        // 플레이어 위치 이동
        if (Player.Instance != null && nextZone.spawnPoint != null)
        {
            Player.Instance.transform.position = nextZone.spawnPoint.position;
        }

        currentZone.OnEnter();

        // 화면 밝아짐
        yield return FadeController.Instance.FadeIn();

        isTransitioning = false;
    }
}
