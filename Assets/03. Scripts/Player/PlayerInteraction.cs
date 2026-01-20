using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentTarget;

    // ?말풍선을 위한 이벤트 발행
    public event Action<bool> OnInteractableStateChanged;


    private void Update()
    {
        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
        {
            return;
        }

        if (Time.timeScale == 0f)
            return;

        if (currentTarget == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTarget.Interact();
        }
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter: " + other.name);

        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentTarget = interactable;
            OnInteractableStateChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentTarget)
        {
            currentTarget = null;
            OnInteractableStateChanged?.Invoke(false);
        }
    }
}
