global using Point = (int, int);

namespace Aoc2024.Utils
{
    public class Grid<T>(T[][] _grid)
    {
        private readonly T[][] grid = _grid;

        public Point[] FindNeighbors(Point pointCoords, Point[]? toAdd = null)
        {
            toAdd ??= [(0, -1), (1, 0), (0, 1), (-1, 0)];

            var (x, y) = pointCoords;

            return
            [
                .. toAdd
                    .Select(
                        (toAddPoint) =>
                        {
                            var (toAddX, toAddY) = toAddPoint;
                            return (x + toAddX, y + toAddY);
                        }
                    )
                    .Where(IsValidPoint),
            ];
        }

        public bool IsValidPoint(Point pointCoords)
        {
            var (x, y) = pointCoords;

            var maxY = grid.Length - 1;
            var maxX = grid[0].Length - 1;

            return x >= 0 && x <= maxX && y >= 0 && y <= maxY;
        }

        public T GetGridValue(Point pointCoords)
        {
            var (x, y) = pointCoords;
            return grid[y][x];
        }

        public Point[] FindAllInGrid(T value)
        {
            List<Point> points = [];

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (
                        (grid[y][x] == null && value == null)
                        || (grid[y][x]?.Equals(value) ?? false)
                    )
                    {
                        points.Add((x, y));
                    }
                }
            }

            return [.. points];
        }

        public Point FindInGrid(T value)
        {
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (
                        (grid[y][x] == null && value == null)
                        || (grid[y][x]?.Equals(value) ?? false)
                    )
                    {
                        return (x, y);
                    }
                }
            }

            throw new InvalidOperationException($"Value {value} not found in grid.");
        }

        public void ForEach(Action<T> callback)
        {
            ForEachCoord((coord) => callback(GetGridValue(coord)));
        }

        public void ForEachCoord(Action<Point> callback)
        {
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    callback((x, y));
                }
            }
        }

        public Grid<T> CloneWith(Point coordToChange, T value)
        {
            var newGrid = grid.Select((row) => (T[])row.Clone()).ToArray();

            var (x, y) = coordToChange;
            newGrid[y][x] = value;

            return new Grid<T>(newGrid);
        }

        public (int, int) GetSize()
        {
            return (grid[0].Length, grid.Length);
        }
    }
}
