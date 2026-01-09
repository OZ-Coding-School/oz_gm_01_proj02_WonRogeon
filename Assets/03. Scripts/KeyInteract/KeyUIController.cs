using UnityEngine;
using UnityEngine.UI;

public class KeyUIController : MonoBehaviour
{
    [SerializeField] private Image keyIconImage;

    private void Awake()
    {
        keyIconImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (KeyInventory.Instance != null)
        {
            KeyInventory.Instance.OnKeyCountChanged += OnKeyCountChanged;
            OnKeyCountChanged(KeyInventory.Instance.KeyCount);
        }
    }

    private void OnDisable()
    {
        if (KeyInventory.Instance != null)
            KeyInventory.Instance.OnKeyCountChanged -= OnKeyCountChanged;
    }

    private void OnKeyCountChanged(int count)
    {
        keyIconImage.gameObject.SetActive(count > 0);
    }
}
