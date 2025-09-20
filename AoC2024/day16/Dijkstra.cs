using Aoc2024.Utils;

namespace Aoc2024.Day16;

using State = (Point, Direction);
using VisitedEntry = (int, List<(Point, Direction)>);

public class Dijkstra(HashSet<Point> mazePoints, Point start, Point target)
{
    private readonly Direction startingDirection = Direction.Right;
    private readonly int moveCost = 1;
    private readonly int rotationCost = 1000;

    public int FindShortestPathCost()
    {
        return RunDijkstra((visited) => visited.First(kv => kv.Key.Item1 == target).Value.Item1);
    }

    public int CountPointsOnShortestPaths()
    {
        return RunDijkstra(
            (visited) =>
            {
                var uniquePoints = new HashSet<Point>();

                var targetEntries = visited.Where(kv => kv.Key.Item1 == target);
                var minCost = targetEntries.Min(kv => kv.Value.Item1);
                var queue = targetEntries
                    .Where(kv => kv.Value.Item1 == minCost)
                    .Select(kv => kv.Key)
                    .ToList();

                while (queue.Count > 0)
                {
                    var entry = queue.First();
                    queue.RemoveAt(0);

                    var (position, _) = entry;
                    uniquePoints.Add(position);

                    queue.AddRange(visited[entry].Item2);
                }

                return uniquePoints.Count;
            }
        );
    }

    private T RunDijkstra<T>(Func<Dictionary<State, VisitedEntry>, T> resultExtractor)
    {
        var visited = new Dictionary<State, VisitedEntry>();
        var queue = new PriorityQueue<(State, State?), int>();
        queue.Enqueue(((start, startingDirection), null), 0);

        while (queue.Count > 0)
        {
            queue.TryDequeue(out var currentQueueEntry, out var currentCost);
            var (currentState, previousState) = currentQueueEntry;
            var (currentPosition, currentDirection) = currentState;

            var isAlreadyVisited = visited.TryGetValue(currentState, out var existingEntry);
            if (isAlreadyVisited)
            {
                var (existingCost, existingPredecessors) = existingEntry;
                if (currentCost < existingCost)
                {
                    throw new InvalidOperationException("This should never happen.");
                }

                if (currentCost == existingCost && previousState is not null)
                {
                    existingPredecessors.Add(previousState.Value);
                }
                else if (currentCost > existingCost)
                {
                    if (currentPosition == target)
                    {
                        break;
                    }
                }

                continue;
            }
            else
            {
                visited.Add(
                    currentState,
                    (currentCost, previousState is null ? [] : [previousState.Value])
                );
            }

            var possibleNextPosition = DirectionUtils.GetPointAfterMovement(
                currentPosition,
                currentDirection
            );
            ((State, State?), int)[] possibleNextQueueEntries =
            [
                (((possibleNextPosition, currentDirection), currentState), currentCost + moveCost),
                (
                    ((currentPosition, DirectionUtils.TurnLeft(currentDirection)), currentState),
                    currentCost + rotationCost
                ),
                (
                    ((currentPosition, DirectionUtils.TurnRight(currentDirection)), currentState),
                    currentCost + rotationCost
                ),
            ];

            var nextQueueEntries = possibleNextQueueEntries.Where(
                (queueEntry) =>
                {
                    var (((position, _), _), _) = queueEntry;

                    return mazePoints.Contains(position);
                }
            );
            foreach (var (state, cost) in nextQueueEntries)
            {
                queue.Enqueue(state, cost);
            }
        }

        return resultExtractor(visited);
    }
}
