using Aoc2024.Utils;

namespace Aoc2024.Day10
{
    public class TrailMap(Grid<int> map)
    {
        private readonly int highestElevation = 9;
        private HashSet<Point> visitedWithHighestElevation = [];

        public int SumUpTrailheadScores()
        {
            var scoreSum = 0;
            var trailheads = map.FindAllInGrid(0);

            foreach (var trailhead in trailheads)
            {
                visitedWithHighestElevation = [];
                TraverseHikingTrails(trailhead);
                var score = visitedWithHighestElevation.Count;
                scoreSum += score;
            }

            visitedWithHighestElevation = [];
            return scoreSum;
        }

        private void TraverseHikingTrails(
            Point point,
            int? previousElevation = null,
            Point? previousPoint = null
        )
        {
            var currentElevation = map.GetGridValue(point);

            if (previousElevation != null)
            {
                if (currentElevation != previousElevation + 1)
                {
                    return;
                }

                if (currentElevation == highestElevation)
                {
                    visitedWithHighestElevation.Add(point);
                    return;
                }
            }

            var neighbors = map.FindNeighbors(point);
            foreach (var neighbor in neighbors)
            {
                if (neighbor != previousPoint)
                {
                    TraverseHikingTrails(neighbor, currentElevation);
                }
            }
        }

        public int SumUpTrailheadsHikingTrailsCount()
        {
            var trailheads = map.FindAllInGrid(0);

            return trailheads.Sum((trailhead) => CountTrailheadsHikingTrails(trailhead));
        }

        private int CountTrailheadsHikingTrails(
            Point point,
            int? previousElevation = null,
            Point? previousPoint = null
        )
        {
            var currentElevation = map.GetGridValue(point);

            if (previousElevation != null)
            {
                if (currentElevation != previousElevation + 1)
                {
                    return 0;
                }

                if (currentElevation == highestElevation)
                {
                    return 1;
                }
            }

            var neighbors = map.FindNeighbors(point);
            return neighbors.Sum(
                (neighbor) => CountTrailheadsHikingTrails(neighbor, currentElevation, point)
            );
        }
    }
}
