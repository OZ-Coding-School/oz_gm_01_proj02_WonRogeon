using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject doorTilemap;
    [SerializeField] private string requiredKeyId = "Zone4Key";
    [SerializeField] private DoorStateProvider doorState;

    private bool doorOpened;
    private bool pendingOpen;

    /// <summary>
    /// PlayerInteraction에서 Space 입력 시 호출됨
    /// </summary>
    public void Interact()
    {
        if (doorOpened)
            return;

        // 메시지가 떠 있는 상태라면
        // 닫기 / 후속 처리만 담당
        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
        {
            MessageUI.Instance.Hide();

            if (pendingOpen)
            {
                OpenDoor();
            }

            return;
        }

        // 메시지가 없을 때만 문 열기 시도
        TryOpenDoor();
    }

    private void TryOpenDoor()
    {
        if (!KeyInventory.Instance.HasKey(requiredKeyId))
        {
            MessageUI.Instance.Show(
                "The door seems locked.\nIt looks like Aya need a key."
            );
            return;
        }

        pendingOpen = true;

        MessageUI.Instance.Show(
            "Aya opened the locked door with a key."
        );
    }

    private void OpenDoor()
    {
        pendingOpen = false;
        doorOpened = true;

        if (doorState != null)
            doorState.Unlock();

        KeyInventory.Instance.RemoveKey(requiredKeyId);

        if (doorTilemap != null)
            doorTilemap.SetActive(false);
    }
}
