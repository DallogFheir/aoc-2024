using Aoc2024.Utils;

namespace Aoc2024.Day07
{
    using Expression = (long, long[]);
    using Operator = Func<long, long, long>;

    public class Solution
    {
        private static readonly Operator add = (a, b) => a + b;
        private static readonly Operator multiply = (a, b) => a * b;
        private static readonly Operator concatenate = (a, b) =>
        {
            var aStr = a.ToString();
            var bStr = b.ToString();
            return long.Parse(aStr + bStr);
        };

        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Assert.ExpectedEqualsActual(3749L, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static long SolvePart1(string inputPath)
        {
            return Solve(
                inputPath,
                (expressions) =>
                    SumUpValidExpressionResults(
                        expressions,
                        new ExpressionValidator([add, multiply])
                    )
            );
        }

        private static long Solve(string inputPath, Func<Expression[], long> solver)
        {
            var lines = FileOpener.ReadIntoSplitLines<Expression>(
                $"day07/{inputPath}",
                (line) =>
                {
                    var parts = line.Split(": ");
                    var equationValues = parts[1].Split(" ").Select(long.Parse).ToArray();

                    return (long.Parse(parts[0]), equationValues);
                }
            );
            return solver(lines);
        }

        private static long SumUpValidExpressionResults(
            Expression[] expressions,
            ExpressionValidator expressionValidator
        )
        {
            return FindValidExpressionResults(expressions, expressionValidator).Sum();
        }

        private static long[] FindValidExpressionResults(
            Expression[] expressions,
            ExpressionValidator expressionValidator
        )
        {
            return expressions
                .Where(expressionValidator.IsExpressionValid)
                .Select((expression) => expression.Item1)
                .ToArray();
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test.txt");
            Assert.ExpectedEqualsActual(11387L, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static long SolvePart2(string inputPath)
        {
            return Solve(
                inputPath,
                (expressions) =>
                    SumUpValidExpressionResults(
                        expressions,
                        new ExpressionValidator([add, multiply, concatenate])
                    )
            );
        }
    }
}
