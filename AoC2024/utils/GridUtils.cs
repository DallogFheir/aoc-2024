global using Point = (int, int);

namespace Aoc2024.Utils
{
    public static class GridUtils
    {
        public static Point[] FindNeighbors<T>(T[][] grid, Point pointCoords, Point[] toAdd)
        {
            var (x, y) = pointCoords;

            return toAdd
                .Select(
                    (toAddPoint) =>
                    {
                        var (toAddX, toAddY) = toAddPoint;
                        return (x + toAddX, y + toAddY);
                    }
                )
                .Where((point) => IsValidPoint(grid, point))
                .ToArray();
        }

        private static bool IsValidPoint<T>(T[][] grid, Point pointCoords)
        {
            var (x, y) = pointCoords;

            var maxY = grid.Length - 1;
            var maxX = grid[0].Length - 1;

            return x >= 0 && x <= maxX && y >= 0 && y <= maxY;
        }

        public static T GetGridValue<T>(T[][] grid, Point pointCoords)
        {
            var (x, y) = pointCoords;
            return grid[y][x];
        }
    }
}
