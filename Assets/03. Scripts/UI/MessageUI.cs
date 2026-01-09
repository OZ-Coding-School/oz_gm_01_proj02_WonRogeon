using UnityEngine;
using TMPro;

public class MessageUI : MonoBehaviour
{
    public static MessageUI Instance;

    [SerializeField] private TMP_Text messageText;

    public bool IsShowing { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        messageText.gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        if (IsShowing)
            return;

        IsShowing = true;
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        if (!IsShowing)
            return;

        IsShowing = false;
        messageText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
