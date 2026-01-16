using UnityEngine;

// 아야의 머리위에 위치할 ?말풍선 아이콘
public class InteractionIconUI : MonoBehaviour
{
    [SerializeField] private GameObject iconRoot;
    [SerializeField] private PlayerInteraction playerInteraction;

    private void Awake()
    {
        iconRoot.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerInteraction != null)
            playerInteraction.OnInteractableStateChanged += SetVisible;
    }

    private void OnDisable()
    {
        if (playerInteraction != null)
            playerInteraction.OnInteractableStateChanged -= SetVisible;
    }

    private void SetVisible(bool visible)
    {
        iconRoot.SetActive(visible);
    }
}
