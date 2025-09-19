using Aoc2024.Utils;

namespace Aoc2024.Day16;

using State = (Point, Direction);

public class Dijkstra(HashSet<Point> mazePoints, Point start, Point target)
{
    private readonly Direction startingDirection = Direction.Right;
    private readonly int moveCost = 1;
    private readonly int rotationCost = 1000;

    public int FindShortestPathCost()
    {
        var visited = new HashSet<State>();
        var queue = new PriorityQueue<State, int>();
        queue.Enqueue((start, startingDirection), 0);

        while (queue.Count > 0)
        {
            queue.TryDequeue(out var currentState, out var currentCost);
            var (currentPosition, currentDirection) = currentState;

            if (currentPosition == target)
            {
                return currentCost;
            }

            visited.Add(currentState);

            var possibleNextPosition = DirectionUtils.GetPointAfterMovement(
                currentPosition,
                currentDirection
            );
            (State, int)[] possibleNextQueueEntries =
            [
                ((possibleNextPosition, currentDirection), currentCost + moveCost),
                (
                    (currentPosition, DirectionUtils.TurnLeft(currentDirection)),
                    currentCost + rotationCost
                ),
                (
                    (currentPosition, DirectionUtils.TurnRight(currentDirection)),
                    currentCost + rotationCost
                ),
            ];

            var nextQueueEntries = possibleNextQueueEntries.Where(
                (queueEntry) =>
                {
                    var (state, _) = queueEntry;
                    var (position, _) = state;

                    return mazePoints.Contains(position) && !visited.Contains(state);
                }
            );
            foreach (var (state, cost) in nextQueueEntries)
            {
                queue.Enqueue(state, cost);
            }
        }

        throw new InvalidOperationException("No path found");
    }
}
