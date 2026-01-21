using UnityEngine;
using System.Collections.Generic;

public class PuzzleImageRandomizer : MonoBehaviour
{
    [SerializeField] private Transform puzzleGridRoot;

    private List<Transform> slots = new();
    private List<ImagePuzzlePiece> pieces = new();

    public void Randomize()
    {
        CacheSlots();
        CachePieces();

        // 정답 슬롯 저장 (섞기 직전)
        foreach (var piece in pieces)
        {
            piece.SetCorrectSlot(piece.transform.parent);
        }

        // 전부 분리
        foreach (var piece in pieces)
            piece.transform.SetParent(null, false);

        Shuffle(slots);

        // 다시 랜덤 배치
        for (int i = 0; i < pieces.Count; i++)
            pieces[i].transform.SetParent(slots[i], false);
    }

    private void CacheSlots()
    {
        slots.Clear();
        foreach (Transform child in puzzleGridRoot)
            slots.Add(child);
    }

    private void CachePieces()
    {
        pieces.Clear();
        pieces.AddRange(
            puzzleGridRoot.GetComponentsInChildren<ImagePuzzlePiece>(true)
        );
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
