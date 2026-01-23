using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentTarget;

    // ? 말풍선용 이벤트
    public event Action<bool> OnInteractableStateChanged;

    [Header("Interaction SFX")]
    [SerializeField] private AudioClip interactSFX;

    private void Update()
    {
        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
            return;

        if (Time.timeScale == 0f)
            return;

        if (currentTarget == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 기본 상호작용 소리
            if (SoundManager.Instance != null && interactSFX != null)
            {
                SoundManager.Instance.PlayGameSFXAt(
                    transform.position,
                    interactSFX
                );
            }

            currentTarget.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentTarget = interactable;
            OnInteractableStateChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentTarget)
        {
            currentTarget = null;
            OnInteractableStateChanged?.Invoke(false);
        }
    }
}
