using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject doorTilemap;
    [SerializeField] private string requiredKeyId = "퍼즐방 열쇠";
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
}
