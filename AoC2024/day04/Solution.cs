using Aoc2024.Utils;

namespace Aoc2024.Day04
{
    public class Solution
    {
        private static readonly string wordToFind = "XMAS";

        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Assert.ExpectedEqualsActual(18, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, CountWord);
        }

        private static int Solve(string inputPath, Func<char[][], int> solver)
        {
            var grid = FileOpener.ReadIntoGrid<char>($"day04/{inputPath}");
            return solver(grid);
        }

        private static int CountWord(char[][] grid)
        {
            Point[] toAddHorizontally = Enumerable
                .Range(0, wordToFind.Length)
                .Select((xCoord) => (xCoord, 0))
                .ToArray();
            Point[] toAddVertically = Enumerable
                .Range(0, wordToFind.Length)
                .Select((yCoord) => (0, yCoord))
                .ToArray();
            Point[] toAddDiagonallyLeft = Enumerable
                .Range(0, wordToFind.Length)
                .Select((coord) => (-coord, coord))
                .ToArray();
            Point[] toAddDiagonallyRight = Enumerable
                .Range(0, wordToFind.Length)
                .Select((coord) => (coord, coord))
                .ToArray();
            Point[][] toAddCoordSets =
            [
                toAddHorizontally,
                toAddVertically,
                toAddDiagonallyLeft,
                toAddDiagonallyRight,
            ];

            var count = 0;
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    Array.ForEach(
                        toAddCoordSets,
                        (toAddCoordSet) =>
                        {
                            var neighbors = GridUtils.FindNeighbors(grid, (x, y), toAddCoordSet);

                            if (neighbors.Length == toAddCoordSet.Length)
                            {
                                var word = string.Join(
                                    "",
                                    neighbors.Select(
                                        (coords) => GridUtils.GetGridValue(grid, coords)
                                    )
                                );

                                if (StringUtils.CompareOrderInsensitive(word, wordToFind))
                                {
                                    count++;
                                }
                            }
                        }
                    );
                }
            }

            return count;
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test.txt");
            Assert.ExpectedEqualsActual(9, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath, CountXMas);
        }

        private static int CountXMas(char[][] grid)
        {
            var wordToFind = "MAS";

            Point[] toAddCoords = [(0, 0), (2, 0), (0, 2), (2, 2), (1, 1)];

            var count = 0;
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    var neighbors = GridUtils.FindNeighbors(grid, (x, y), toAddCoords);

                    if (neighbors.Length == toAddCoords.Length)
                    {
                        var letters = neighbors
                            .Select((coords) => GridUtils.GetGridValue(grid, coords))
                            .ToArray();

                        var topLeftCorner = letters[0].ToString();
                        var topRightCorner = letters[1].ToString();
                        var bottomLeftCorner = letters[2].ToString();
                        var bottomRightCorner = letters[3].ToString();
                        var middle = letters[4].ToString();

                        var word1 = topLeftCorner + middle + bottomRightCorner;
                        var word2 = topRightCorner + middle + bottomLeftCorner;

                        if (
                            StringUtils.CompareOrderInsensitive(word1, wordToFind)
                            && StringUtils.CompareOrderInsensitive(word2, wordToFind)
                        )
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }
    }
}
