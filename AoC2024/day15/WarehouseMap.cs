using Aoc2024.Utils;

namespace Aoc2024.Day15;

public class WarehouseMap
{
    private static readonly char WALL_TILE_CHAR = '#';
    private static readonly char ROBOT_TILE_CHAR = '@';
    private static readonly char EMPTY_TILE_CHAR = '.';
    private static readonly char BOX_TILE_CHAR = 'O';
    private static readonly char BOX_LEFT_TILE_CHAR = '[';
    private static readonly char BOX_RIGHT_TILE_CHAR = ']';

    private readonly HashSet<Point> wallCoordinates = [];
    private readonly Dictionary<Point, MovableObject> coordinatesToBoxes = [];
    private readonly MovableObject robot;
    private readonly int maxX;
    private readonly int maxY;

    public WarehouseMap(string[] mapLines)
    {
        maxX = mapLines[0].Length - 1;
        maxY = mapLines.Length - 1;

        for (int rowIdx = 0; rowIdx < mapLines.Length; rowIdx++)
        {
            for (int colIdx = 0; colIdx < mapLines[rowIdx].Length; colIdx++)
            {
                var mapCharacter = mapLines[colIdx][rowIdx];

                if (mapCharacter == ROBOT_TILE_CHAR)
                {
                    robot = new MovableObject((rowIdx, colIdx), 1);
                    continue;
                }

                if (mapCharacter == EMPTY_TILE_CHAR)
                {
                    continue;
                }

                if (mapCharacter == WALL_TILE_CHAR)
                {
                    wallCoordinates.Add((rowIdx, colIdx));
                    continue;
                }

                if (mapCharacter == BOX_TILE_CHAR)
                {
                    var box = new MovableObject((rowIdx, colIdx), 1);
                    coordinatesToBoxes[(rowIdx, colIdx)] = box;
                    continue;
                }

                if (mapCharacter == BOX_LEFT_TILE_CHAR)
                {
                    var firstPoint = (rowIdx, colIdx);
                    var secondPoint = (rowIdx, colIdx + 1);

                    var box = new MovableObject(firstPoint, 2);
                    coordinatesToBoxes[firstPoint] = box;
                    coordinatesToBoxes[secondPoint] = box;

                    continue;
                }

                if (mapCharacter == BOX_RIGHT_TILE_CHAR)
                {
                    var firstPoint = (rowIdx, colIdx - 1);

                    if (!coordinatesToBoxes.ContainsKey(firstPoint))
                    {
                        throw new ArgumentException("Found ']' without matching '['.");
                    }

                    continue;
                }

                throw new ArgumentException($"Invalid map character: {mapCharacter}");
            }
        }
        if (robot == null)
        {
            throw new ArgumentException("Robot starting position not found on the map.");
        }
    }

    public int SumBoxGpsCoordinates()
    {
        var boxes = new HashSet<MovableObject>(coordinatesToBoxes.Values);
        return boxes.Sum(box => box.GetGpsCoordinate());
    }

    public void MoveRobot(Direction[] movements)
    {
        foreach (var movement in movements)
        {
            bool canPerformMovement = CanPerformMovementForObjects(movement, [robot]);

            if (canPerformMovement)
            {
                PerformMovement(movement, [robot], true);
            }
        }
    }

    private bool CanPerformMovementForObjects(Direction movement, MovableObject[] objects)
    {
        if (objects.Length == 0)
        {
            return true;
        }

        List<MovableObject> newObjects = [];

        foreach (var obj in objects)
        {
            var newPositions = obj.GetPositionsAfterMovement(movement);

            bool isOutOfBounds = newPositions.Any(pos =>
                pos.Item1 < 0 || pos.Item1 > maxX || pos.Item2 < 0 || pos.Item2 > maxY
            );
            if (isOutOfBounds)
            {
                throw new InvalidOperationException("Movement out of bounds.");
            }

            bool canMove = newPositions.All(pos => !wallCoordinates.Contains(pos));
            if (!canMove)
            {
                return false;
            }
            newObjects.AddRange(
                newPositions
                    .Where(coordinatesToBoxes.ContainsKey)
                    .Select(pos => coordinatesToBoxes[pos])
            );
        }

        return CanPerformMovementForObjects(movement, [.. newObjects.Distinct()]);
    }

    private void PerformMovement(Direction movement, MovableObject[] objects, bool doUpdateRobot)
    {
        if (objects.Length == 0)
        {
            return;
        }

        foreach (var obj in objects)
        {
            var oldPositions = obj.LeftmostPoint;
            var newPositions = obj.GetPositionsAfterMovement(movement);

            obj.Move(movement);

            if (!doUpdateRobot)
            {
                Enumerable
                    .Range(0, obj.Width)
                    .ToList()
                    .ForEach(offset =>
                    {
                        var oldPos = (oldPositions.Item1 + offset, oldPositions.Item2);
                        coordinatesToBoxes.Remove(oldPos);
                    });
            }

            PerformMovement(
                movement,
                [
                    .. newPositions
                        .Where(coordinatesToBoxes.ContainsKey)
                        .Select(pos => coordinatesToBoxes[pos]),
                ],
                false
            );

            if (!doUpdateRobot)
            {
                newPositions
                    .ToList()
                    .ForEach(pos =>
                    {
                        coordinatesToBoxes[pos] = obj;
                    });
            }
        }
    }
}
