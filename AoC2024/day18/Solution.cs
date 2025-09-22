using Aoc2024.Utils;

namespace Aoc2024.Day18
{
    public class Solution
    {
        public static void Part1()
        {
            var testResult = SolvePart1("test.txt", 6, 12);
            Assert.ExpectedEqualsActual(22, testResult);

            var result = SolvePart1("input.txt", 70, 1024);
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath, int gridMaxCoordinate, int byteCount)
        {
            return Solve(
                inputPath,
                (lines) =>
                {
                    var corruptedPoints = new HashSet<Point>(lines.Take(byteCount));

                    var memorySpace = new MemorySpace(
                        gridMaxCoordinate,
                        gridMaxCoordinate,
                        (gridMaxCoordinate, gridMaxCoordinate),
                        corruptedPoints
                    );

                    return memorySpace.FindShortestPathLength();
                }
            );
        }

        private static int Solve(string inputPath, Func<Point[], int> solver)
        {
            var lines = FileOpener.ReadIntoSplitLines(
                $"day18/{inputPath}",
                (line) =>
                {
                    var parts = line.Split(',').Select(int.Parse).ToArray();

                    return (parts[0], parts[1]);
                }
            );

            return solver(lines);
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
