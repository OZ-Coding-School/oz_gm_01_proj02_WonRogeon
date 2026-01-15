using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class PuzzleAnswerData
{
    public PuzzlePiece piece;
    public Vector2Int correctPos;
}

public class PuzzleAnswerChecker : MonoBehaviour
{
    public static event Action OnPuzzleSolved;

    [SerializeField] private List<PuzzleAnswerData> answers = new();

    private bool solved;

    public void CheckAnswer()
    {
        if (solved) return;

        foreach (var a in answers)
        {
            if (!a.piece.HasPlaced)
                return;

            if (a.piece.CellPos != a.correctPos)
                return;
        }

        solved = true;
        OnPuzzleSolved?.Invoke();
    }
}
