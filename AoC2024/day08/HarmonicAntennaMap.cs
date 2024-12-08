using Aoc2024.Utils;

namespace Aoc2024.Day08
{
    public class HarmonicAntennaMap(Grid<char> map) : AntennaMap(map)
    {
        protected override Point[] FindAntinodesForPairOfAntennas(Point antenna1, Point antenna2)
        {
            var (x1, y1) = antenna1;
            var (x2, y2) = antenna2;

            var xDist = x1 - x2;
            var yDist = y1 - y2;

            List<Point> antinodes = [(x1, y1)];

            var possibleAntinode = (x1 + xDist, y1 + yDist);
            while (IsPointInMap(possibleAntinode))
            {
                antinodes.Add(possibleAntinode);
                possibleAntinode = (possibleAntinode.Item1 + xDist, possibleAntinode.Item2 + yDist);
            }

            possibleAntinode = (x1 - xDist, y1 - yDist);
            while (IsPointInMap(possibleAntinode))
            {
                antinodes.Add(possibleAntinode);
                possibleAntinode = (possibleAntinode.Item1 - xDist, possibleAntinode.Item2 - yDist);
            }

            return [.. antinodes];
        }
    }
}
