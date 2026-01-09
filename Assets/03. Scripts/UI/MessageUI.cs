using UnityEngine;
using TMPro;

public class MessageUI : MonoBehaviour
{
    public static MessageUI Instance;

    [SerializeField] private TMP_Text messageText;

    public bool IsShowing { get; private set; }

    private bool waitForRelease;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        messageText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsShowing)
            return;

        if (waitForRelease)
        {
            if (Input.GetKeyUp(KeyCode.Space))
                waitForRelease = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hide();
        }
    }

    public void Show(string message)
    {
        if (IsShowing)
            return;

        IsShowing = true;
        waitForRelease = true;

        messageText.text = message;
        messageText.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

    private void Hide()
    {
        IsShowing = false;
        messageText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
