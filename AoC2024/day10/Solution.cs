using Aoc2024.Utils;

namespace Aoc2024.Day10
{
    public class Solution
    {
        public static void Part1()
        {
            var testResult1 = SolvePart1("test1.txt");
            Assert.ExpectedEqualsActual(1, testResult1);

            var testResult2 = SolvePart1("test2.txt");
            Assert.ExpectedEqualsActual(36, testResult2);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, (map) => new TrailMap(map).SumUpTrailheadScores());
        }

        private static int Solve(string inputPath, Func<Grid<int>, int> solver)
        {
            var map = FileOpener.ReadIntoGrid(
                $"day10/{inputPath}",
                (el) => (int)char.GetNumericValue(el)
            );
            return solver(map);
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test2.txt");
            Assert.ExpectedEqualsActual(81, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath, (map) => new TrailMap(map).SumUpTrailheadsHikingTrailsCount());
        }
    }
}
