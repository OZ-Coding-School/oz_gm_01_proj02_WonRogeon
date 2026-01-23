using UnityEngine;

public class KeyPickupInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string keyId = "퍼즐방 열쇠";

    [Header("SFX")]
    [SerializeField] private AudioClip pickupSFX; 

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

        if (pickupSFX != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameSFXAt(
                transform.position,
                pickupSFX
            );
        }

        MessageUI.Instance.Show(
            $"아야는 {keyId}를 획득했다.",
            () => gameObject.SetActive(false)
        );
    }
}
