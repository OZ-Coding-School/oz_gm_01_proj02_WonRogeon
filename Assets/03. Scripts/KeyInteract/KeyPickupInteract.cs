using UnityEngine;

public class KeyPickupInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string keyId = "Zone4Key";

    private bool pickedUp;
    private bool pendingDisable;

    public void Interact()
    {
        // 메시지가 떠 있으면 최우선 처리
        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
        {
            MessageUI.Instance.Hide();

            if (pendingDisable)
            {
                gameObject.SetActive(false);
            }

            return;
        }

        // 이미 획득한 상태면 아무 것도 하지 않음
        if (pickedUp)
            return;

        TryPickupKey();
    }

    private void TryPickupKey()
    {
        pickedUp = true;

        if (KeyInventory.Instance.AddKey(keyId))
        {
            pendingDisable = true;
            MessageUI.Instance.Show($"Aya get a {keyId}.");
        }
        else
        {
            pickedUp = false;
        }
    }
}
