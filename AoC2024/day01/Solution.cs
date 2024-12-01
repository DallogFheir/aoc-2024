using System.Diagnostics;
using Aoc2024.Utils;

namespace Aoc2024.Day01
{
    public static class Solution
    {
        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Debug.Assert(testResult == 11);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, FindTotalDistance);
        }

        private static int Solve(string inputPath, Func<int[], int[], int> solver)
        {
            var lines = FileOpener.ReadIntoSplitLines($"day01/{inputPath}", (line) => line.Split("   "));

            var leftList = lines.Select((line) => int.Parse(line[0])).ToArray();
            var rightList = lines.Select((line) => int.Parse(line[1])).ToArray();

            return solver(leftList, rightList);
        }

        private static int FindTotalDistance(int[] leftLocationList, int[] rightLocationList)
        {
            var leftListSorted = leftLocationList.OrderBy(locationId => locationId);
            var rightListSorted = rightLocationList.OrderBy(locationId => locationId);

            var result = leftListSorted.Zip(rightListSorted).Sum((zippedLists) =>
            {
                var (leftLocationId, rightLocationId) = zippedLists;

                return Math.Abs(leftLocationId - rightLocationId);
            });

            return result;
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test.txt");
            Debug.Assert(testResult == 31);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath, FindSimilarityScore);
        }

        private static int FindSimilarityScore(int[] leftLocationList, int[] rightLocationList)
        {
            var rightListLocationIdCounter = rightLocationList.GroupBy((locationId) => locationId).ToDictionary((group) => group.Key, (group) => group.Count());

            return leftLocationList.Sum((locationId) =>
            {
                var locationIdInRightListCount = rightListLocationIdCounter.GetValueOrDefault(locationId, 0);
                return locationIdInRightListCount * locationId;
            });
        }
    }
}
