using UnityEngine;

/// <summary>
/// 메인 씬 전용 BGM 재생 컨트롤러
/// - 메인 씬 진입 시 1회 BGM 재생
/// - Zone 전환과 무관
/// </summary>
public class MainSceneBGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip mainBGM;

    private void Start()
    {
        if (SoundManager.Instance == null || mainBGM == null)
            return;

        SoundManager.Instance.PlayBGM(mainBGM);
    }
}
