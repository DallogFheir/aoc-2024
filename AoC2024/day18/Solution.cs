using Aoc2024.Utils;

namespace Aoc2024.Day18
{
    public class Solution
    {
        public static void Part1()
        {
            var testResult = SolvePart1("test.txt", 6, 12);
            Assert.ExpectedEqualsActual("22", testResult);

            var result = SolvePart1("input.txt", 70, 1024);
            Console.WriteLine($"Part 1: {result}");
        }

        private static string SolvePart1(string inputPath, int gridMaxCoordinate, int byteCount)
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

                    return memorySpace.FindShortestPathLength().ToString();
                }
            );
        }

        private static string Solve(string inputPath, Func<Point[], string> solver)
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
            var testResult = SolvePart2("test.txt", 6, 12 + 1);
            Assert.ExpectedEqualsActual("6,1", testResult);

            var result = SolvePart2("input.txt", 70, 1024 + 1);
            Console.WriteLine($"Part 2: {result}");
        }

        private static string SolvePart2(
            string inputPath,
            int gridMaxCoordinate,
            int startByteCount
        )
        {
            return Solve(
                inputPath,
                (lines) =>
                {
                    for (int i = startByteCount; i <= lines.Length; i++)
                    {
                        var corruptedPoints = new HashSet<Point>(lines.Take(i));

                        var memorySpace = new MemorySpace(
                            gridMaxCoordinate,
                            gridMaxCoordinate,
                            (gridMaxCoordinate, gridMaxCoordinate),
                            corruptedPoints
                        );

                        try
                        {
                            memorySpace.FindShortestPathLength();
                        }
                        catch (InvalidOperationException)
                        {
                            var (x, y) = lines[i - 1];
                            return $"{x},{y}";
                        }
                    }

                    throw new InvalidOperationException("No solution found.");
                }
            );
        }
    }
}
