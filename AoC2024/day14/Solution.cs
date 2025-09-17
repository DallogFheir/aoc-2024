using System.Drawing;
using Aoc2024.Utils;

namespace Aoc2024.Day14
{
    public class Solution
    {
        private static readonly int NUM_OF_SECONDS = 100;
        private static readonly int TEST_MAX_X = 10;
        private static readonly int TEST_MAX_Y = 6;
        private static readonly int INPUT_MAX_X = 100;
        private static readonly int INPUT_MAX_Y = 102;

        public static void Part1()
        {
            var testResult = SolvePart1("test.txt", TEST_MAX_X, TEST_MAX_Y);
            Assert.ExpectedEqualsActual(12, testResult);

            var result = SolvePart1("input.txt", INPUT_MAX_X, INPUT_MAX_Y);
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
            return GetRobotCountForQuadrantsAfterSeconds(robots, NUM_OF_SECONDS)
                .Values.Aggregate((product, current) => product * current);
        }

        private static Dictionary<Quadrant, int> GetRobotCountForQuadrantsAfterSeconds(
            Robot[] robots,
            int numOfSeconds
        )
        {
            var robotCountForQuadrant = new Dictionary<Quadrant, int>();

            foreach (var robot in robots)
            {
                robot.Move(numOfSeconds);

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

            return robotCountForQuadrant;
        }

        public static void Part2()
        {
            var result = Solve("input.txt", INPUT_MAX_X, INPUT_MAX_Y, FindChristmasTree);
            Console.WriteLine($"Part 2: {result}");
        }

        private static int FindChristmasTree(Robot[] robots)
        {
            // found by experimenting
            var secondsPassedThreshold = 6771;
            var differenceThreshold = 70;
            var step = 1;

            for (int i = 1; i * step <= secondsPassedThreshold; i++)
            {
                foreach (var robot in robots)
                {
                    robot.Move(step);
                }

                var robotCountForQuadrants = GetRobotCountForQuadrantsAfterSeconds(robots, 0);
                var countsSorted = robotCountForQuadrants.Values.OrderDescending().ToArray();

                var densestQuadrantCount = countsSorted[0];
                var secondDensestQuadrantCount = countsSorted[1];
                var quadrantDensityDifference = densestQuadrantCount - secondDensestQuadrantCount;
                if (quadrantDensityDifference >= differenceThreshold)
                {
                    GenerateImage(robots);
                    return i * step;
                }
            }

            throw new Exception("Christmas tree not found.");
        }

        private static void GenerateImage(Robot[] robots)
        {
            var imageWidth = INPUT_MAX_X + 1;
            var imageHeight = INPUT_MAX_Y + 1;
            var robotColor = Color.Black;

            using Bitmap bitmap = new(imageWidth, imageHeight);
            using Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            foreach (var robot in robots)
            {
                bitmap.SetPixel(robot.Position.X, robot.Position.Y, robotColor);
            }

            bitmap.Save($@"day14\christmas_tree.png");
        }
    }
}
