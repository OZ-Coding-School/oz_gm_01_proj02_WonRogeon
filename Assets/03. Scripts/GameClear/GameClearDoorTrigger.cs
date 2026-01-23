using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameClearDoorTrigger : MonoBehaviour
{
    [SerializeField] private string clearSceneName = "GameClearScene";

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;
        StartCoroutine(ClearRoutine());
    }

    private IEnumerator ClearRoutine()
    {
        Coroutine bgmFade = null;

        // 1. BGM 페이드 아웃 + 정지 (동시 시작)
        if (SoundManager.Instance != null)
        {
            bgmFade = SoundManager.Instance.FadeOutAndStopBGM(this);
        }

        // 2. 화면 페이드 아웃
        yield return FadeController.Instance.FadeOut();

        // 3. BGM 페이드가 아직 끝나지 않았다면 대기
        if (bgmFade != null)
            yield return bgmFade;

        // 4. 씬 전환
        SceneManager.LoadScene(clearSceneName);
    }

}
