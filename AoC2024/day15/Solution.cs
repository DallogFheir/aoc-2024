using Aoc2024.Utils;

namespace Aoc2024.Day15
{
    public class Solution
    {
        public static void Part1()
        {
            var test1Result = SolvePart1("test1.txt");
            Assert.ExpectedEqualsActual(2028, test1Result);
            var test2Result = SolvePart1("test2.txt");
            Assert.ExpectedEqualsActual(10092, test2Result);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath);
        }

        private static int Solve(string inputPath, Func<string, string>? mapMapper = null)
        {
            mapMapper ??= (line) => line;

            var (mapStr, movementsStr) = FileOpener.ReadIntoTwoParts($"day15/{inputPath}");

            var map = new WarehouseMap([.. mapStr.Split("\n").Select(line => mapMapper(line))]);
            var movements = movementsStr
                .Replace("\n", "")
                .Select(ParseCharacterToMovement)
                .ToArray();

            map.MoveRobot(movements);
            return map.SumBoxGpsCoordinates();
        }

        private static Direction ParseCharacterToMovement(char chr)
        {
            return chr switch
            {
                '^' => Direction.Up,
                'v' => Direction.Down,
                '<' => Direction.Left,
                '>' => Direction.Right,
                _ => throw new ArgumentException($"Invalid movement character: {chr}"),
            };
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test2.txt");
            Assert.ExpectedEqualsActual(9021, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath);
        }
    }
}
