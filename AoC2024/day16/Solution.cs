using Aoc2024.Utils;

namespace Aoc2024.Day16
{
    public class Solution
    {
        private static readonly char WALL_CHAR = '#';
        private static readonly char STARTING_POSITION_CHAR = 'S';
        private static readonly char TARGET_CHAR = 'E';

        public static void Part1()
        {
            var test1Result = SolvePart1("test1.txt");
            Assert.ExpectedEqualsActual(7036, test1Result);
            var test2Result = SolvePart1("test2.txt");
            Assert.ExpectedEqualsActual(11048, test2Result);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, (dijkstra) => dijkstra.FindShortestPathCost());
        }

        private static int Solve(string inputPath, Func<Dijkstra, int> solver)
        {
            var lines = FileOpener.ReadIntoSplitLines<string>($"day16/{inputPath}");

            var mazePoints = new HashSet<Point>();
            Point? startingPosition = null;
            Point? target = null;
            for (int rowIdx = 0; rowIdx < lines.Length; rowIdx++)
            {
                for (int colIdx = 0; colIdx < lines[rowIdx].Length; colIdx++)
                {
                    var tile = lines[rowIdx][colIdx];

                    if (tile == WALL_CHAR)
                    {
                        continue;
                    }

                    var point = (colIdx, rowIdx);
                    mazePoints.Add(point);

                    if (tile == STARTING_POSITION_CHAR)
                    {
                        startingPosition = point;
                    }
                    else if (tile == TARGET_CHAR)
                    {
                        target = point;
                    }
                }
            }

            if (startingPosition == null || target == null)
            {
                throw new InvalidOperationException("Missing starting position or target.");
            }

            var dijkstra = new Dijkstra(mazePoints, startingPosition.Value, target.Value);
            return solver(dijkstra);
        }

        public static void Part2()
        {
            // var testResult = SolvePart2("test.txt");
            // Assert.ExpectedEqualsActual(expectedValue, testResult);

            // var result = SolvePart2("input.txt");
            // Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            // return Solve(inputPath, solver);
            return 0;
        }
    }
}
