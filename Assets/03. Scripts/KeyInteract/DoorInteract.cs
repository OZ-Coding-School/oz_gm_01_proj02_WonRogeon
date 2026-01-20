using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject doorTilemap;
    [SerializeField] private string requiredKeyId = "Zone4Key";
    [SerializeField] private DoorStateProvider doorState;

    private bool doorOpened;

    public void Interact()
    {
        if (doorOpened)
            return;

        // 메시지가 떠 있으면 닫기만
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
            MessageUI.Instance.Show(
                "The door seems locked.\nIt looks like Aya need a key."
            );
            return;
        }

        OpenDoor();

        MessageUI.Instance.Show(
            "Aya opened the locked door with a key."
        );
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
}
