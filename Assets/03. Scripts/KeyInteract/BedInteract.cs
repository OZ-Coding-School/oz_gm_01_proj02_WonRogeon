using UnityEngine;

public class BedInteract : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (SaveMenuUI.Instance != null && SaveMenuUI.Instance.IsOpen)
            return;

        SaveMenuUI.Instance.Open();
    }


    public string GetInteractText()
    {
        return "ÀáÀÚ±â";
    }
}
