using UnityEngine;
using TMPro;

public class PuzzlePanelController : MonoBehaviour
{
    [SerializeField] private GameObject[] views;
    [SerializeField] private TMP_Text viewTitleText;
    [SerializeField] private string[] viewTitles;


    private int currentIndex = 0;

    private void OnEnable()
    {
        currentIndex = 0;
        RefreshView();
    }

    public void OnClickNext()
    {
        if (currentIndex >= views.Length - 1)
            return;

        currentIndex++;
        RefreshView();
    }

    public void OnClickPrev()
    {
        if (currentIndex <= 0)
            return;

        currentIndex--;
        RefreshView();
    }

    private void RefreshView()
    {
        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetActive(i == currentIndex);
        }

        if (viewTitleText != null && viewTitles != null && currentIndex < viewTitles.Length)
        {
            viewTitleText.text = viewTitles[currentIndex];
        }
    }
}
