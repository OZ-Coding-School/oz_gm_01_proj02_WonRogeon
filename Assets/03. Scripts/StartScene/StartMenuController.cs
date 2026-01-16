using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text[] menuTexts;
    [SerializeField] private RectTransform selectionBar;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.black;

    [Header("Scene")]
    [SerializeField] private string mainSceneName = "MainScene";

    private bool isTransitioning = false;
    private int currentIndex = 0;
    private float inputCooldown = 0.01f;
    private float lastInputTime;

    private void Start()
    {
        UpdateVisual();
    }

    private void Update()
    {
        if (isTransitioning)
            return;

        HandleMoveInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentIndex == 0)
            {
                StartCoroutine(StartGameRoutine());
            }
        }
    }

    private IEnumerator StartGameRoutine()
    {
        isTransitioning = true;

        yield return FadeController.Instance.FadeOut();

        SceneManager.LoadScene(mainSceneName);
    }
    private void HandleMoveInput()
    {
        if (Time.time - lastInputTime < inputCooldown)
            return;

        int dir = 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            dir = -1;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            dir = 1;

        if (dir == 0)
            return;

        currentIndex += dir;

        if (currentIndex < 0)
            currentIndex = menuTexts.Length - 1;
        else if (currentIndex >= menuTexts.Length)
            currentIndex = 0;

        lastInputTime = Time.time;
        UpdateVisual();
    }

    private void HandleSubmitInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (currentIndex == 0) // New Game
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else if (currentIndex == 4) // Quit
        {
            Application.Quit();
        }
    }

    private void UpdateVisual()
    {
        for (int i = 0; i < menuTexts.Length; i++)
        {
            menuTexts[i].color =
                (i == currentIndex) ? selectedColor : normalColor;
        }

        selectionBar.position =
            new Vector3(
                selectionBar.position.x,
                menuTexts[currentIndex].transform.position.y,
                selectionBar.position.z
            );
    }
}
