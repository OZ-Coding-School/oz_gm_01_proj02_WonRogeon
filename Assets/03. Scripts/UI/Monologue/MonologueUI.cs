using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MonologueUI : MonoBehaviour
{
    public static MonologueUI Instance;

    [Header("UI")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image portraitImage;

    [Header("Database")]
    [SerializeField] private CharacterPortraitDatabase portraitDB;

    private Queue<MonologueLine> lines = new();
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

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsShowing)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }

    // ===== 寇何 龋免 =====
    public void ShowSequence(List<MonologueLine> sequence)
    {
        lines.Clear();
        foreach (var l in sequence)
            lines.Enqueue(l);

        Time.timeScale = 0f;
        IsShowing = true;
        gameObject.SetActive(true);

        ShowNextLine();
    }

    // ===== 郴何 贸府 =====
    private void ShowNextLine()
    {
        if (lines.Count == 0)
        {
            Hide();
            return;
        }

        var line = lines.Dequeue();

        text.text = line.message;

        var sprite = portraitDB.GetPortrait(
            line.character,
            line.expression
        );

        portraitImage.sprite = sprite;
        portraitImage.gameObject.SetActive(sprite != null);
    }

    private void Hide()
    {
        IsShowing = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}

[System.Serializable]
public class MonologueLine
{
    public string character;
    public string expression;
    [TextArea] public string message;
}
