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
                var mapCharacter = mapLines[rowIdx][colIdx];

                if (mapCharacter == ROBOT_TILE_CHAR)
                {
                    robot = new MovableObject((colIdx, rowIdx), 1);
                    continue;
                }

                if (mapCharacter == EMPTY_TILE_CHAR)
                {
                    continue;
                }

                if (mapCharacter == WALL_TILE_CHAR)
                {
                    wallCoordinates.Add((colIdx, rowIdx));
                    continue;
                }

                if (mapCharacter == BOX_TILE_CHAR)
                {
                    var point = (colIdx, rowIdx);
                    var box = new MovableObject(point, 1);
                    coordinatesToBoxes[point] = box;
                    continue;
                }

                if (mapCharacter == BOX_LEFT_TILE_CHAR)
                {
                    var firstPoint = (colIdx, rowIdx);
                    var secondPoint = (colIdx + 1, rowIdx);

                    var box = new MovableObject(firstPoint, 2);
                    coordinatesToBoxes[firstPoint] = box;
                    coordinatesToBoxes[secondPoint] = box;

                    continue;
                }

                if (mapCharacter == BOX_RIGHT_TILE_CHAR)
                {
                    var firstPoint = (colIdx - 1, rowIdx);

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
            var oldPositions = obj.GetPositions();
            var newPositions = obj.GetPositionsAfterMovement(movement)
                .Where((pos) => !oldPositions.Contains(pos));

            bool isOutOfBounds = newPositions.Any(pos =>
                pos.Item1 < 0 || pos.Item1 > maxX || pos.Item2 < 0 || pos.Item2 > maxY
            );
            if (isOutOfBounds)
            {
                throw new InvalidOperationException("Movement out of bounds.");
            }

            bool isBlockedByWall = newPositions.Any(wallCoordinates.Contains);
            if (isBlockedByWall)
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
            var oldPositions = obj.GetPositions();
            var newPositions = obj.GetPositionsAfterMovement(movement);

            obj.Move(movement);

            if (!doUpdateRobot)
            {
                oldPositions
                    .ToList()
                    .ForEach(pos =>
                    {
                        coordinatesToBoxes.Remove(pos);
                    });
            }

            PerformMovement(
                movement,
                [
                    .. newPositions
                        .Where(pos =>
                            !oldPositions.Contains(pos) && coordinatesToBoxes.ContainsKey(pos)
                        )
                        .Select(pos => coordinatesToBoxes[pos])
                        .Distinct(),
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
