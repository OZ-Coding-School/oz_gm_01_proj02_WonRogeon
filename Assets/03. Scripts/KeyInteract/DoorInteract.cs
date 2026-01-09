using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] private GameObject doorTilemap;
    [SerializeField] private string requiredKeyId = "Zone4Key";

    private bool inRange;
    private bool doorOpened;
    private bool pendingOpen;

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

        // 메시지가 떠 있으면: Space로 닫기만 처리
        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MessageUI.Instance.Hide();

                // 닫은 직후 문을 여는 단계라면 여기서 처리
                if (pendingOpen)
                {
                    pendingOpen = false;
                    doorOpened = true;
                    KeyInventory.Instance.RemoveKey(requiredKeyId);
                    if (doorTilemap != null)
                        doorTilemap.SetActive(false);
                }
            }
            return;
        }

        // 메시지가 없을 때만 상호작용
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
