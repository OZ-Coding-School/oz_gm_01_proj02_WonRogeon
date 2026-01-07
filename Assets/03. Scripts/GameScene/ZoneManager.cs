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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeZone(Zone nextZone)
    {
        if (nextZone == null)
            return;

        StartCoroutine(ChangeZoneRoutine(nextZone));
    }

    private IEnumerator ChangeZoneRoutine(Zone nextZone)
    {
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
    }
}
