// DoorInteract.cs
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] private GameObject doorTilemap;
    [SerializeField] private string requiredKeyId = "Zone4Key";

    private bool inRange = false;
    private bool doorOpened = false;
    private bool pendingOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inRange = false;
    }

    private void Update()
    {
        if (!inRange || doorOpened)
            return;

        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
            return;

        if (pendingOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pendingOpen = false;
                doorOpened = true;

                KeyInventory.Instance.RemoveKey(requiredKeyId);

                if (doorTilemap != null)
                    doorTilemap.SetActive(false);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryOpenDoor();
        }
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
}
