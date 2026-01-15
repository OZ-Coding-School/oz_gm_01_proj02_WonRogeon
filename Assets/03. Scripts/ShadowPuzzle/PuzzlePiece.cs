using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;

    [HideInInspector] public Vector2Int CellPos;
    [HideInInspector] public bool HasPlaced;
}
