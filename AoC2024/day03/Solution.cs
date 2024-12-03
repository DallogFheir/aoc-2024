using System.Text.RegularExpressions;
using Aoc2024.Utils;

namespace Aoc2024.Day03
{
    public class Solution
    {
        private static readonly Dictionary<string, string> instructionRegexes = new()
        {
            { "mul", @"(mul)\((\d{1,3},\d{1,3})\)" },
            { "do", @"(do)\(\)" },
            { "don't", @"(don't)\(\)" },
        };

        public static void Part1()
        {
            var testResult = SolvePart1("test1.txt");
            Assert.ExpectedEqualsActual(161, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, (program) => ExecuteCorrectInstructions(program, [instructionRegexes["mul"]]));
        }

        private static int Solve(string inputPath, Func<string, int> solver)
        {
            var program = string.Join("", FileOpener.ReadIntoSplitLines<string>($"day03/{inputPath}").Select((line) => line[0]));
            return solver(program);
        }

        private static int ExecuteCorrectInstructions(string program, string[] correctInstructionRegexes)
        {
            var instructions = FindCorrectInstructions(program, correctInstructionRegexes);

            return new InstructionExecutor(instructions).Execute();
        }

        private static (string, int[])[] FindCorrectInstructions(string program, string[] correctInstructionRegexes)
        {
            var instructionPattern = string.Join('|', correctInstructionRegexes);

            var instructionMatches = Regex.Matches(program, instructionPattern);
            return instructionMatches.Select((instructionMatch) =>
            {
                var operationName = instructionMatch.Groups[1].Value;
                int[] operands = [];

                if (operationName == "")
                {
                    operationName = instructionMatch.Groups[3].Success ? instructionMatch.Groups[3].Value : instructionMatch.Groups[4].Value;
                }
                else
                {
                    var operandsMatch = instructionMatch.Groups[2];
                    operands = operandsMatch.Value.Split(",").Select((operandStr) => int.Parse(operandStr)).ToArray();
                }

                return (operationName, operands);
            }).ToArray();
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test2.txt");
            Assert.ExpectedEqualsActual(48, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath, (program) => ExecuteCorrectInstructions(program, [.. instructionRegexes.Values]));
        }
    }
}
