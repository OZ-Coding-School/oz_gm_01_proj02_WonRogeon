// KeyPickupInteract.cs
using UnityEngine;

public class KeyPickupInteract : MonoBehaviour
{
    [SerializeField] private string keyId = "Zone4Key";

    private bool inRange = false;
    private bool pickedUp = false;

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
        if (!inRange || pickedUp)
            return;

        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pickedUp = true;

            if (KeyInventory.Instance.AddKey(keyId))
            {
                MessageUI.Instance.Show($"Aya get a {keyId}.");
                gameObject.SetActive(false);
            }
        }
    }
}
