using UnityEngine;

/// <summary>
/// 전체 플레이 시간 관리 전용 클래스
/// - Time.timeScale과 무관하게 누적
/// - 저장/불러오기용 총 플레이 시간 제공
/// </summary>
public class PlayTimeTracker : MonoBehaviour
{
    public static PlayTimeTracker Instance;

    // 누적 플레이 시간 (초)
    private float playTimeSeconds;

    public float PlayTimeSeconds => playTimeSeconds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // UI 열림(TimeScale = 0) 상태에서도 시간은 흐르게 처리
        playTimeSeconds += Time.unscaledDeltaTime;
    }

    /// <summary>
    /// 저장 데이터 로드 시 플레이 시간 강제 설정
    /// </summary>
    public void SetPlayTime(float seconds)
    {
        playTimeSeconds = Mathf.Max(0f, seconds);
    }

    /// <summary>
    /// 저장 슬롯 표시용 hh:mm:ss 문자열 반환
    /// </summary>
    public string GetFormattedTime()
    {
        var time = System.TimeSpan.FromSeconds(playTimeSeconds);
        return time.ToString(@"hh\:mm\:ss");
    }
}
