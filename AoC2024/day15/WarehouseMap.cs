using Aoc2024.Day14;

namespace Aoc2024.Day15;

public class WarehouseMap
{
    private static readonly Dictionary<char, MapTile> CHAR_TO_MAP_TILE = new()
    {
        { '#', MapTile.Wall },
        { 'O', MapTile.Box },
    };
    private static readonly char ROBOT_TILE_CHAR = '@';
    private static readonly char EMPTY_TILE_CHAR = '.';

    private readonly Dictionary<(int, int), MapTile> map = [];
    private (int, int) robotLocation;
    private readonly int maxX;
    private readonly int maxY;

    public WarehouseMap(string[] mapLines)
    {
        maxX = mapLines[0].Length - 1;
        maxY = mapLines.Length - 1;

        for (int i = 0; i < mapLines.Length; i++)
        {
            for (int j = 0; j < mapLines[i].Length; j++)
            {
                var mapCharacter = mapLines[j][i];

                if (mapCharacter == ROBOT_TILE_CHAR)
                {
                    robotLocation = (j, i);
                    continue;
                }

                if (mapCharacter == EMPTY_TILE_CHAR)
                {
                    continue;
                }

                var wasTileTypeRetrieved = CHAR_TO_MAP_TILE.TryGetValue(
                    mapCharacter,
                    out var tileType
                );
                if (!wasTileTypeRetrieved)
                {
                    throw new ArgumentException($"Invalid map character: {mapCharacter}");
                }

                map[(i, j)] = tileType;
            }
        }
    }

    public int SumBoxGpsCoordinates()
    {
        return map.Sum(
            (coordinateTileTypePair) =>
            {
                var coordinate = coordinateTileTypePair.Key;
                var tileType = coordinateTileTypePair.Value;

                return tileType == MapTile.Box ? CalculateGpsCoordinate(coordinate) : 0;
            }
        );
    }

    private static int CalculateGpsCoordinate((int, int) coordinate)
    {
        return coordinate.Item1 + 100 * coordinate.Item2;
    }

    public void MoveRobot(Movement[] movements)
    {
        foreach (var movement in movements)
        {
            var newRobotLocation = GetPositionAfterMovement(robotLocation, movement);
            if (newRobotLocation.HasValue)
            {
                robotLocation = newRobotLocation.Value;
                MoveBox(robotLocation, movement);
            }
        }
    }

    private (int, int)? GetPositionAfterMovement((int, int) from, Movement direction)
    {
        var to = direction switch
        {
            Movement.Up => (from.Item1, from.Item2 - 1),
            Movement.Down => (from.Item1, from.Item2 + 1),
            Movement.Left => (from.Item1 - 1, from.Item2),
            Movement.Right => (from.Item1 + 1, from.Item2),
            _ => throw new Exception("This will never happen."),
        };

        var isOutOfBounds = to.Item1 < 0 || to.Item1 > maxX || to.Item2 < 0 || to.Item2 > maxY;
        if (isOutOfBounds)
        {
            throw new InvalidOperationException(
                "Moved out of bounds - this should never happen, as the walls should enclose the whole map."
            );
        }

        var isEmptyTile = !map.TryGetValue(to, out var tileAtNewLocation);
        if (isEmptyTile)
        {
            return to;
        }

        if (tileAtNewLocation == MapTile.Wall)
        {
            return null;
        }

        var furtherMovement = GetPositionAfterMovement(to, direction);
        return furtherMovement.HasValue ? to : null;
    }

    private void MoveBox((int, int) stoneLocation, Movement direction)
    {
        var isEmptyTile = !map.TryGetValue(stoneLocation, out var tileAtStoneLocation);
        if (isEmptyTile || tileAtStoneLocation != MapTile.Box)
        {
            return;
        }

        var newStoneLocation = GetPositionAfterMovement(stoneLocation, direction);
        if (!newStoneLocation.HasValue)
        {
            throw new ArgumentException("Stone cannot be moved in the given direction.");
        }

        MoveBox(newStoneLocation.Value, direction);
        map.Remove(stoneLocation);
        map[newStoneLocation.Value] = MapTile.Box;
    }
}
