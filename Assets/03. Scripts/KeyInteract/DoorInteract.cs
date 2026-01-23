using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject doorTilemap;
    [SerializeField] private string requiredKeyId = "퍼즐방 열쇠";
    [SerializeField] private DoorStateProvider doorState;

    [Header("SFX")]
    [SerializeField] private AudioClip lockedSFX;

    private bool doorOpened;

    public void Interact()
    {
        if (doorOpened)
            return;

        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
        {
            MessageUI.Instance.Hide();
            return;
        }

        TryOpenDoor();
    }

    private void TryOpenDoor()
    {
        if (!KeyInventory.Instance.HasKey(requiredKeyId))
        {
            // 잠김 사운드
            PlayLockedSFX();

            MessageUI.Instance.Show(
                "문이 잠겨있다. 열쇠가 필요해 보인다."
            );
            return;
        }

        MessageUI.Instance.Show(
            "실험실에서 가져온 열쇠가 딱 맞는다."
        );
        OpenDoor();
    }

    private void OpenDoor()
    {
        doorOpened = true;

        if (doorState != null)
            doorState.Unlock();

        KeyInventory.Instance.RemoveKey(requiredKeyId);

        if (doorTilemap != null)
            doorTilemap.SetActive(false);

        gameObject.SetActive(false);
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
