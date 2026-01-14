using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentTarget;

    private void Update()
    {
        if (currentTarget == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTarget.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentTarget = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentTarget)
        {
            currentTarget = null;
        }
    }
}
