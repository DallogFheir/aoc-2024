using Aoc2024.Utils;

namespace Aoc2024.Day11
{
    public class Solution
    {
        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Assert.ExpectedEqualsActual(55312, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, (stones) => BlinkAtStones(stones, 25));
        }

        private static int Solve(string inputPath, Func<long[], int> solver)
        {
            var stones = FileOpener.ReadIntoSplitLines(
                $"day11/{inputPath}",
                (line) => line.Split(" ").Select((num) => long.Parse(num)).ToArray()
            )[0];
            return solver(stones);
        }

        private static int BlinkAtStones(long[] stonesArray, int howManyTimes)
        {
            HashSet<Stone> stones = stonesArray
                .Select((stoneNumber) => new Stone(stoneNumber))
                .ToHashSet();

            for (int i = 0; i < howManyTimes; i++)
            {
                HashSet<Stone> newStones = [];

                foreach (var stone in stones)
                {
                    var changedStones = stone.Change();

                    foreach (var changedStone in changedStones)
                    {
                        newStones.Add(changedStone);
                    }
                }

                stones = newStones;
            }

            return stones.Count;
        }

        public static void Part2()
        {
            // var result = SolvePart2("input.txt");
            // Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            // return Solve(inputPath, (stones) => BlinkAtStones(stones, 75));
            return 0;
        }
    }
}
