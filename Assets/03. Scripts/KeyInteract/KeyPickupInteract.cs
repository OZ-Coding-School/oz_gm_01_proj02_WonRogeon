using UnityEngine;

public class KeyPickupInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string keyId = "Zone4Key";

    private bool pickedUp;

    private void Start()
    {
        SyncWithInventory();
    }

    /// <summary>
    /// 로드 이후 / 씬 시작 시
    /// 인벤토리 상태와 동기화
    /// </summary>
    public void SyncWithInventory()
    {
        if (KeyInventory.Instance == null)
            return;

        if (KeyInventory.Instance.HasKey(keyId))
        {
            pickedUp = true;
            gameObject.SetActive(false);
        }
        else
        {
            pickedUp = false;
            gameObject.SetActive(true);
        }
    }

    public void Interact()
    {
        if (pickedUp)
            return;

        TryPickupKey();
    }

    private void TryPickupKey()
    {
        if (!KeyInventory.Instance.AddKey(keyId))
            return;

        pickedUp = true;

        MessageUI.Instance.Show(
            $"Aya get a {keyId}.",
            () => gameObject.SetActive(false)
        );
    }
}
