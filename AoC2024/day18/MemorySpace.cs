namespace Aoc2024.Day18;

public class MemorySpace(int maxX, int maxY, Point target, HashSet<Point> corruptedPoints)
{
    HashSet<Point> visited = [];

    public int FindShortestPathLength()
    {
        visited = [];

        var startingPoint = (0, 0);

        var queue = new Queue<(Point, int)>();
        queue.Enqueue((startingPoint, 0));

        while (queue.Count > 0)
        {
            var (currentPoint, currentStepCount) = queue.Dequeue();

            if (IsTarget(currentPoint))
            {
                return currentStepCount;
            }

            foreach (var neighbor in GetNeighbors(currentPoint))
            {
                if (IsValidMove(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, currentStepCount + 1));
                }
            }
        }

        throw new InvalidOperationException("No path found to the target.");
    }

    private bool IsTarget(Point point)
    {
        return point == target;
    }

    private static Point[] GetNeighbors(Point point)
    {
        return
        [
            (point.Item1 + 1, point.Item2),
            (point.Item1 - 1, point.Item2),
            (point.Item1, point.Item2 + 1),
            (point.Item1, point.Item2 - 1),
        ];
    }

    private bool IsValidMove(Point point)
    {
        return point.Item1 >= 0
            && point.Item1 <= maxX
            && point.Item2 >= 0
            && point.Item2 <= maxY
            && !corruptedPoints.Contains(point)
            && !visited.Contains(point);
    }
}
