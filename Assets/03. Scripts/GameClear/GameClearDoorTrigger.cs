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
        yield return FadeController.Instance.FadeOut();
        SceneManager.LoadScene(clearSceneName);
    }
}
