using System.Diagnostics;
using Aoc2024.Utils;

namespace Aoc2024.Day05
{
    public class Solution
    {
        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Assert.ExpectedEqualsActual(143, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, SumUpMiddlePagesOfCorrectUpdates);
        }

        private static int Solve(string inputPath, Func<(int, int)[], int[][], int> solver)
        {
            var (rulesArr, instructions) = FileOpener.ReadIntoTwoPartsThenSplitLines(
                $"day05/{inputPath}",
                (line) => line.Split("|").Select((numStr) => int.Parse(numStr)).ToArray(),
                (line) => line.Split(",").Select((numStr) => int.Parse(numStr)).ToArray()
            );

            var rules = rulesArr.Select((ruleArr) => (ruleArr[0], ruleArr[1])).ToArray();

            return solver(rules, instructions);
        }

        private static int SumUpMiddlePagesOfCorrectUpdates(
            (int, int)[] rules,
            int[][] instructions
        )
        {
            var rulesMap = ParseRules(rules);

            return instructions.Sum(
                (instruction) =>
                {
                    if (!new InstructionChecker(instruction, rulesMap).IsInstructionValid())
                    {
                        return 0;
                    }

                    Debug.Assert(instruction.Length % 2 == 1);
                    var middleIdx = instruction.Length / 2;
                    return instruction[middleIdx];
                }
            );
        }

        private static Dictionary<int, HashSet<int>> ParseRules((int, int)[] rules)
        {
            Dictionary<int, HashSet<int>> pageToPagesAfter = [];

            Array.ForEach(
                rules,
                (rule) =>
                {
                    var (before, after) = rule;

                    var pagesAfter = pageToPagesAfter.GetValueOrDefault(before, []);
                    pagesAfter.Add(after);
                    pageToPagesAfter[before] = pagesAfter;
                }
            );

            return pageToPagesAfter;
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test.txt");
            Assert.ExpectedEqualsActual(123, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath, SumUpMiddlePagesOfCorrectedIncorrectUpdates);
        }

        private static int SumUpMiddlePagesOfCorrectedIncorrectUpdates(
            (int, int)[] rules,
            int[][] instructions
        )
        {
            var rulesMap = ParseRules(rules);
            return instructions
                .Where(
                    (instruction) =>
                        !new InstructionChecker(instruction, rulesMap).IsInstructionValid()
                )
                .Sum(
                    (instruction) =>
                    {
                        Graph<int> ruleGraph = new(
                            rules
                                .Where(
                                    (rule) =>
                                    {
                                        var (source, target) = rule;

                                        return instruction.Contains(source)
                                            && instruction.Contains(target);
                                    }
                                )
                                .ToArray()
                        );
                        var sortedPages = ruleGraph.SortTopologically();

                        var correctedInstruction = instruction
                            .OrderBy((page) => Array.IndexOf(sortedPages, page))
                            .ToArray();

                        Debug.Assert(instruction.Length % 2 == 1);
                        var middleIdx = correctedInstruction.Length / 2;
                        return correctedInstruction[middleIdx];
                    }
                );
        }
    }
}
