using Aoc2024.Utils;

namespace Aoc2024.Day06
{
    public class Solution
    {
        private static readonly char obstacle = '#';
        private static readonly char startingGuard = '^';

        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Assert.ExpectedEqualsActual(41, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static int SolvePart1(string inputPath)
        {
            return Solve(inputPath, (map) => FindAllGuardPositions(map).Count);
        }

        private static int Solve(string inputPath, Func<Grid<char>, int> solver)
        {
            var map = FileOpener.ReadIntoGrid($"day06/{inputPath}");
            return solver(map);
        }

        private static HashSet<Point> FindAllGuardPositions(Grid<char> map)
        {
            var currentGuardPosition = map.FindInGrid(startingGuard);
            var currentDirection = Direction.Up;
            Dictionary<Point, HashSet<Direction>> visited = new()
            {
                { currentGuardPosition, [Direction.Up] },
            };

            while (true)
            {
                var newGuardPosition = DirectionUtils.GetPointAfterMovement(
                    currentGuardPosition,
                    currentDirection
                );

                if (!map.IsValidPoint(newGuardPosition))
                {
                    break;
                }

                if (
                    visited.GetValueOrDefault(newGuardPosition)?.Contains(currentDirection) ?? false
                )
                {
                    throw new InvalidOperationException("Guard is in a loop.");
                }

                if (map.GetGridValue(newGuardPosition) == obstacle)
                {
                    currentDirection = DirectionUtils.TurnRight(currentDirection);
                    continue;
                }

                var directionsForCoord = visited.GetValueOrDefault(newGuardPosition, []);
                directionsForCoord.Add(currentDirection);
                visited[newGuardPosition] = directionsForCoord;

                currentGuardPosition = newGuardPosition;
            }

            return [.. visited.Keys];
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test.txt");
            Assert.ExpectedEqualsActual(6, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static int SolvePart2(string inputPath)
        {
            return Solve(inputPath, CountPossibleNewObstacles);
        }

        private static int CountPossibleNewObstacles(Grid<char> map)
        {
            var allGuardsPositions = FindAllGuardPositions(map);
            var count = 0;

            foreach (var coord in allGuardsPositions)
            {
                var (x, y) = coord;

                var valueAtCoord = map.GetGridValue(coord);
                if (valueAtCoord != startingGuard)
                {
                    var newMap = map.CloneWith(coord, obstacle);

                    try
                    {
                        FindAllGuardPositions(newMap);
                    }
                    catch (InvalidOperationException)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
