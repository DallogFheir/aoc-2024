using Aoc2024.Utils;

namespace Aoc2024.Day14
{
    public class Solution
    {
        private static readonly int NUM_OF_SECONDS = 100;

        public static void Part1()
        {
            var testResult = SolvePart1("test.txt", 10, 6);
            Assert.ExpectedEqualsActual(12, testResult);

            var result = SolvePart1("input.txt", 100, 102);
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath, int maxX, int maxY)
        {
            Func<Robot[], int> solver = CalculateSafetyFactor;
            return Solve(inputPath, maxX, maxY, solver);
        }

        private static int Solve(string inputPath, int maxX, int maxY, Func<Robot[], int> solver)
        {
            var robots = FileOpener.ReadIntoSplitLines(
                $"day14/{inputPath}",
                new InputParser(maxX, maxY).Parse
            );
            return solver(robots);
        }

        private static int CalculateSafetyFactor(Robot[] robots)
        {
            var robotCountForQuadrant = new Dictionary<Quadrant, int>();

            foreach (var robot in robots)
            {
                robot.Move(NUM_OF_SECONDS);

                var robotQuadrant = robot.GetQuadrant();

                if (robotQuadrant == Quadrant.None)
                {
                    continue;
                }

                var isInDict = robotCountForQuadrant.TryGetValue(robotQuadrant, out var count);
                if (isInDict)
                {
                    robotCountForQuadrant[robotQuadrant] = count + 1;
                }
                else
                {
                    robotCountForQuadrant[robotQuadrant] = 1;
                }
            }

            return robotCountForQuadrant.Values.Aggregate((product, current) => product * current);
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
