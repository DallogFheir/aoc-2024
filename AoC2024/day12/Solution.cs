using Aoc2024.Utils;

namespace Aoc2024.Day12
{
    using Plot = (Point plotCoords, Direction[] outsideEdges);

    public class Solution
    {
        public static void Part1()
        {
            long[] expectedValues = [140, 772, 1930];
            for (int i = 0; i < expectedValues.Length; i++)
            {
                var expectedValue = expectedValues[i];
                var testResult = SolvePart1($"test{i + 1}.txt");
                Assert.ExpectedEqualsActual(expectedValue, testResult);
            }

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static long SolvePart1(string inputPath)
        {
            return Solve(inputPath, CalculateFencePrice);
        }

        private static long Solve(string inputPath, Func<Grid<char>, long> solver)
        {
            var garden = FileOpener.ReadIntoGrid($"day12/{inputPath}");
            return solver(garden);
        }

        private static long CalculateFencePrice(Grid<char> garden)
        {
            return GetGardenRegions(garden)
                .Sum(
                    (regionPlots) =>
                    {
                        var area = regionPlots.Count();

                        static int lengthExtractor(Plot plot) => plot.outsideEdges.Length;

                        var perimeter = regionPlots.Sum(lengthExtractor);

                        return area * perimeter;
                    }
                );
        }

        private static List<IEnumerable<Plot>> GetGardenRegions(Grid<char> garden)
        {
            HashSet<Point> visited = [];
            List<IEnumerable<Plot>> gardenRegions = [];

            garden.ForEachCoord(
                (plotCoord) =>
                {
                    if (visited.Contains(plotCoord))
                    {
                        return;
                    }

                    var plotSymbol = garden.GetGridValue(plotCoord);
                    var regionPlots = ProcessGardenRegion(garden, visited, plotCoord, plotSymbol);
                    gardenRegions.Add(regionPlots);
                }
            );

            return gardenRegions;
        }

        private static List<Plot> ProcessGardenRegion(
            Grid<char> garden,
            HashSet<Point> visited,
            Point plotCoord,
            char plotSymbol
        )
        {
            if (visited.Contains(plotCoord))
            {
                return [];
            }

            visited.Add(plotCoord);

            var plotNeighbors = garden.FindNeighbors(plotCoord);
            var plotContinuation = plotNeighbors.Where(
                (neighbor) => garden.GetGridValue(neighbor) == plotSymbol
            );
            var neighboringEdges = plotContinuation
                .Select((neighbor) => GetDirectionOfMovement(plotCoord, neighbor))
                .ToHashSet();
            var outsideEdges = new List<Direction>(
                [Direction.Left, Direction.Right, Direction.Up, Direction.Down]
            )
                .Where((direction) => !neighboringEdges.Contains(direction))
                .ToArray();

            var thisPlot = (plotCoord, outsideEdges);

            return plotContinuation.Aggregate(
                new List<Plot>([thisPlot]),
                (acc, neighbor) =>
                {
                    var neighborPlot = ProcessGardenRegion(garden, visited, neighbor, plotSymbol);

                    return [.. acc, .. neighborPlot];
                }
            );
        }

        private static Direction GetDirectionOfMovement(Point from, Point to)
        {
            var (xFrom, yFrom) = from;
            var (xTo, yTo) = to;

            return (xFrom - xTo, yFrom - yTo) switch
            {
                (1, 0) => Direction.Left,
                (-1, 0) => Direction.Right,
                (0, 1) => Direction.Up,
                (0, -1) => Direction.Down,
                _ => throw new InvalidOperationException(
                    $"Points {from} and {to} are not neighbors."
                ),
            };
        }

        public static void Part2()
        {
            long[] expectedValues = [80, 436, 1206, 236, 368];
            for (int i = 0; i < expectedValues.Length; i++)
            {
                var expectedValue = expectedValues[i];
                var testResult = SolvePart2($"test{i + 1}.txt");
                Assert.ExpectedEqualsActual(expectedValue, testResult);
            }

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static long SolvePart2(string inputPath)
        {
            return Solve(inputPath, CalculateFencePriceWithDiscount);
        }

        private static long CalculateFencePriceWithDiscount(Grid<char> garden)
        {
            return GetGardenRegions(garden)
                .Sum(
                    (regionPlots) =>
                    {
                        var area = regionPlots.Count();
                        var sideCount = CountSides(garden, regionPlots);

                        return area * sideCount;
                    }
                );
        }

        private static long CountSides(Grid<char> garden, IEnumerable<Plot> plots)
        {
            HashSet<(Point, Direction)> visited = [];
            long sideCount = 0;

            void visitAllColinearEdges(Point plotCoords, Direction edge)
            {
                if (visited.Contains((plotCoords, edge)))
                {
                    return;
                }

                visited.Add((plotCoords, edge));

                var (x, y) = plotCoords;
                var plotSymbol = garden.GetGridValue(plotCoords);

                var neighbors = edge switch
                {
                    Direction.Left or Direction.Right => plots.Where(
                        (otherPlot) =>
                        {
                            var ((otherX, otherY), otherEdge) = otherPlot;
                            return x == otherX
                                && (y == otherY + 1 || y == otherY - 1)
                                && otherEdge.Contains(edge);
                        }
                    ),
                    Direction.Up or Direction.Down => plots.Where(
                        (otherPlot) =>
                        {
                            var ((otherX, otherY), otherEdge) = otherPlot;
                            return (x == otherX + 1 || x == otherX - 1)
                                && y == otherY
                                && otherEdge.Contains(edge);
                        }
                    ),
                    _ => throw new InvalidOperationException($"Invalid direction: {edge}."),
                };

                foreach (var neighbor in neighbors)
                {
                    var (neighborCoords, neighborEdge) = neighbor;
                    visitAllColinearEdges(neighborCoords, edge);
                }
            }

            foreach (var (plotCoords, outsideEdges) in plots)
            {
                foreach (var edge in outsideEdges)
                {
                    var plotWithEdge = (plotCoords, edge);

                    if (visited.Contains(plotWithEdge))
                    {
                        continue;
                    }

                    sideCount++;
                    visitAllColinearEdges(plotCoords, edge);
                }
            }

            return sideCount;
        }
    }
}
