using Aoc2024.Utils;

namespace Aoc2024.Day08
{
    public class BasicAntennaMap(Grid<char> map) : AntennaMap(map)
    {
        protected override Point[] FindAntinodesForPairOfAntennas(Point antenna1, Point antenna2)
        {
            var (x1, y1) = antenna1;
            var (x2, y2) = antenna2;

            static int antinodeCoord(int a1, int a2)
            {
                return 2 * a1 - a2;
            }

            var antinode1 = (antinodeCoord(x1, x2), antinodeCoord(y1, y2));
            var antinode2 = (antinodeCoord(x2, x1), antinodeCoord(y2, y1));

            return [antinode1, antinode2];
        }
    }
}
