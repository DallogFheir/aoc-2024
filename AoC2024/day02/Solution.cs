using Aoc2024.Utils;

namespace Aoc2024.Day02
{
    public class Solution
    {
        private static readonly int maxLevelDifference = 3;

        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Assert.ExpectedEqualsActual(2, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, CountSafeReports);
        }

        private static int Solve(string inputPath, Func<int[][], int> solver)
        {
            var reports = FileOpener.ReadIntoSplitLines($"day02/{inputPath}", (line) => line.Split(" ").Select((level) => int.Parse(level)).ToArray());
            return solver(reports);
        }

        private static int CountSafeReports(int[][] reports)
        {
            return reports.Count(IsReportSafe);
        }

        private static bool IsReportSafe(int[] report)
        {
            bool? isAscending = null;

            foreach (var zippedReports in report.Zip(report.Skip(1)))
            {
                var (prev, next) = zippedReports;

                isAscending ??= prev < next;

                if (!IsSequenceSafe(prev, next, isAscending.Value))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsSequenceSafe(int prev, int next, bool isAscending)
        {
            if (isAscending && prev >= next || !isAscending && prev <= next)
            {
                return false;
            }

            var levelDifference = Math.Abs(prev - next);
            if (levelDifference == 0 || levelDifference > maxLevelDifference)
            {
                return false;
            }

            return true;
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test.txt");
            Assert.ExpectedEqualsActual(4, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath, CountSafeReportsWithProblemDampener);
        }

        private static int CountSafeReportsWithProblemDampener(int[][] reports)
        {
            return reports.Count((report) =>
               {
                   if (IsReportSafe(report))
                   {
                       return true;
                   }

                   return Enumerable.Range(0, report.Length).Any((levelToRemoveIdx) =>
                   {
                       var reportWithoutOneLevel = report.Where((_, idx) => idx != levelToRemoveIdx);
                       return IsReportSafe(reportWithoutOneLevel.ToArray());
                   });
               });
        }
    }
}
