using UnityEngine;

public class KeyPickupInteract : MonoBehaviour
{
    [SerializeField] private string keyId = "Zone4Key";

    private bool inRange;
    private bool pickedUp;
    private bool pendingDisable;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inRange = false;
    }

    private void Update()
    {
        if (!inRange)
            return;

        // 메시지가 떠 있으면: 닫기만 처리
        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MessageUI.Instance.Hide();

                if (pendingDisable)
                {
                    gameObject.SetActive(false);
                }
            }
            return;
        }

        if (pickedUp)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pickedUp = true;

            if (KeyInventory.Instance.AddKey(keyId))
            {
                pendingDisable = true;
                MessageUI.Instance.Show($"Aya get a {keyId}.");
            }
        }
    }

}
