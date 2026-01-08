using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder2D
{
    public class Node
    {
        public Vector2Int pos;
        public int g; // start ¡æ ÇöÀç
        public int h; // heuristic
        public int f => g + h;
        public Node parent;

        public Node(Vector2Int pos)
        {
            this.pos = pos;
        }
    }

    private static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public static List<Vector2> FindPath(
        Vector2 startWorld,
        Vector2 targetWorld,
        float cellSize,
        LayerMask obstacleMask)
    {
        Vector2Int start = WorldToCell(startWorld, cellSize);
        Vector2Int target = WorldToCell(targetWorld, cellSize);

        Dictionary<Vector2Int, Node> open = new();
        Dictionary<Vector2Int, Node> closed = new();

        Node startNode = new(start)
        {
            g = 0,
            h = Heuristic(start, target)
        };

        open[start] = startNode;

        while (open.Count > 0)
        {
            Node current = GetLowestF(open);

            if (current.pos == target)
                return ReconstructPath(current, cellSize);

            open.Remove(current.pos);
            closed[current.pos] = current;

            foreach (var dir in directions)
            {
                Vector2Int nextPos = current.pos + dir;

                if (closed.ContainsKey(nextPos))
                    continue;

                if (IsBlocked(nextPos, cellSize, obstacleMask))
                    continue;

                int newG = current.g + 1;

                if (!open.TryGetValue(nextPos, out Node nextNode))
                {
                    nextNode = new Node(nextPos);
                    open[nextPos] = nextNode;
                }
                else if (newG >= nextNode.g)
                    continue;

                nextNode.g = newG;
                nextNode.h = Heuristic(nextPos, target);
                nextNode.parent = current;
            }
        }

        return null;
    }

    // ===== helpers =====

    private static Vector2Int WorldToCell(Vector2 world, float cellSize)
    {
        return new Vector2Int(
            Mathf.RoundToInt(world.x / cellSize),
            Mathf.RoundToInt(world.y / cellSize)
        );
    }

    private static Vector2 CellToWorld(Vector2Int cell, float cellSize)
    {
        return new Vector2(
            cell.x * cellSize,
            cell.y * cellSize
        );
    }

    private static int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private static Node GetLowestF(Dictionary<Vector2Int, Node> open)
    {
        Node best = null;
        foreach (var n in open.Values)
            if (best == null || n.f < best.f)
                best = n;
        return best;
    }

    private static bool IsBlocked(
        Vector2Int cell,
        float cellSize,
        LayerMask obstacleMask)
    {
        Vector2 world = CellToWorld(cell, cellSize);
        return Physics2D.OverlapBox(
            world,
            Vector2.one * (cellSize * 0.8f),
            0f,
            obstacleMask
        );
    }

    private static List<Vector2> ReconstructPath(Node node, float cellSize)
    {
        List<Vector2> path = new();
        while (node != null)
        {
            path.Add(CellToWorld(node.pos, cellSize));
            node = node.parent;
        }
        path.Reverse();
        return path;
    }
}
