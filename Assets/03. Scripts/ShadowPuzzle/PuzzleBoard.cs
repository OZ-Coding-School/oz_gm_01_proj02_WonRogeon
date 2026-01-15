using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
    public int width = 7;
    public int height = 7;
    public float cellSize = 100f;
    public RectTransform boardRect;

    // 보드 회전값
    public float boardRotationZ = 45f;

    private bool[,] occupied;

    private Quaternion invRotation;

    private void Awake()
    {
        occupied = new bool[width, height];
        invRotation = Quaternion.Euler(0, 0, -boardRotationZ);
    }

    // 피스 로컬 좌표 → 셀
    public Vector2Int LocalPosToCell(Vector2 localPos)
    {
        // 회전 보정
        Vector2 corrected =
            invRotation * localPos;

        Vector2 origin = boardRect.rect.min;

        int x = Mathf.FloorToInt((corrected.x - origin.x) / cellSize);
        int y = Mathf.FloorToInt((corrected.y - origin.y) / cellSize);

        return new Vector2Int(x, y);
    }

    // 셀 → 피스 로컬 좌표
    public Vector2 CellToLocalPos(Vector2Int cell, Vector2Int size)
    {
        Vector2 origin = boardRect.rect.min;

        Vector2 center = origin + new Vector2(
            cell.x * cellSize + size.x * cellSize * 0.5f,
            cell.y * cellSize + size.y * cellSize * 0.5f
        );

        // 다시 회전
        return Quaternion.Euler(0, 0, boardRotationZ) * center;
    }

    public bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public bool IsOccupied(int x, int y)
    {
        return occupied[x, y];
    }

    public void ClearOccupation(PuzzlePiece piece)
    {
        if (!piece.HasPlaced) return;

        for (int x = 0; x < piece.size.x; x++)
            for (int y = 0; y < piece.size.y; y++)
                occupied[piece.CellPos.x + x, piece.CellPos.y + y] = false;

        piece.HasPlaced = false;
    }

    public void Occupy(PuzzlePiece piece, Vector2Int startCell)
    {
        for (int x = 0; x < piece.size.x; x++)
            for (int y = 0; y < piece.size.y; y++)
                occupied[startCell.x + x, startCell.y + y] = true;

        piece.CellPos = startCell;
        piece.HasPlaced = true;
    }

    // 가장 가까운 셀 탐색
    public Vector2Int FindNearestValidCell(PuzzlePiece piece, Vector2 pieceLocalPos)
    {
        float minDist = float.MaxValue;
        Vector2Int best = new(-1, -1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!CanPlace(piece, x, y)) continue;

                Vector2 cellPos = CellToLocalPos(new Vector2Int(x, y), piece.size);
                float d = Vector2.SqrMagnitude(cellPos - pieceLocalPos);

                if (d < minDist)
                {
                    minDist = d;
                    best = new Vector2Int(x, y);
                }
            }
        }

        return best;
    }

    private bool CanPlace(PuzzlePiece piece, int sx, int sy)
    {
        for (int x = 0; x < piece.size.x; x++)
        {
            for (int y = 0; y < piece.size.y; y++)
            {
                int bx = sx + x;
                int by = sy + y;

                if (!IsInsideBoard(bx, by)) return false;
                if (IsOccupied(bx, by)) return false;
            }
        }
        return true;
    }
}
