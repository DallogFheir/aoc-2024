using Aoc2024.Utils;

namespace Aoc2024.Day08
{
    public class Solution
    {
        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Assert.ExpectedEqualsActual(14, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, (grid) => new BasicAntennaMap(grid));
        }

        private static int Solve(string inputPath, Func<Grid<char>, AntennaMap> antennaMapFactory)
        {
            var grid = FileOpener.ReadIntoGrid($"day08/{inputPath}");
            return antennaMapFactory(grid).FindAntinodes().Count;
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test.txt");
            Assert.ExpectedEqualsActual(34, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath, (grid) => new HarmonicAntennaMap(grid));
        }
    }
}
