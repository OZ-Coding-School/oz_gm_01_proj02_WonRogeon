using UnityEngine;

public class ElectricDoorInteract : MonoBehaviour, IInteractable
{
    [Header("SFX")]
    [SerializeField] private AudioClip lockedSFX;

    public void Interact()
    {
        if (MessageUI.Instance.IsShowing)
            return;

        // 잠김 사운드
        PlayLockedSFX();

        MessageUI.Instance.Show(
            "문이 잠겨있다.",
            ShowSecondMessage
        );
    }

    private void ShowSecondMessage()
    {
        MessageUI.Instance.Show(
            "좌측의 패널과 연결되어있는 듯 하다."
        );
    }

    private void PlayLockedSFX()
    {
        if (SoundManager.Instance == null || lockedSFX == null)
            return;

        SoundManager.Instance.PlayGameSFXAt(
            transform.position,
            lockedSFX
        );
    }
}
