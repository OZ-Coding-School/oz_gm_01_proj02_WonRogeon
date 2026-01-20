using TMPro;
using UnityEngine;
using System;

public class MessageUI : MonoBehaviour
{
    public static MessageUI Instance;

    [SerializeField] private TMP_Text messageText;

    public bool IsShowing { get; private set; }
    private Action onClosed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // 루트 자체를 꺼둔다 (MonologueUI와 동일)
        gameObject.SetActive(false);
        IsShowing = false;
    }

    private void Update()
    {
        if (!IsShowing)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hide();
        }
    }

    public void Show(string message, Action onClosed = null)
    {
        if (IsShowing)
            return;

        IsShowing = true;
        this.onClosed = onClosed;

        messageText.text = message;

        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        if (!IsShowing)
            return;

        IsShowing = false;

        gameObject.SetActive(false);
        Time.timeScale = 1f;

        onClosed?.Invoke();
        onClosed = null;
    }
}
