using Aoc2024.Utils;

namespace Aoc2024.Day08
{
    public abstract class AntennaMap
    {
        private static readonly char emptyMapValue = '.';

        private readonly Dictionary<char, List<Point>> antennas = [];
        protected readonly int width;
        protected readonly int height;

        public AntennaMap(Grid<char> map)
        {
            MapGridToAntennas(map);
            (width, height) = map.GetSize();
        }

        private void MapGridToAntennas(Grid<char> map)
        {
            map.ForEachCoord(
                (coord) =>
                {
                    var mapValue = map.GetGridValue(coord);

                    if (mapValue != emptyMapValue)
                    {
                        var antennaLocations = antennas.GetValueOrDefault(mapValue, []);
                        antennaLocations.Add(coord);
                        antennas[mapValue] = antennaLocations;
                    }
                }
            );
        }

        public HashSet<Point> FindAntinodes()
        {
            return antennas
                .Keys.SelectMany(FindAntinodesForFrequency)
                .Where(
                    (antinode) =>
                    {
                        var (x, y) = antinode;

                        return x >= 0 && x < width && y >= 0 && y < height;
                    }
                )
                .ToHashSet();
        }

        private Point[] FindAntinodesForFrequency(char frequency)
        {
            List<Point> antinodes = [];
            var locations = antennas[frequency].ToArray();

            for (int i = 0; i < locations.Length; i++)
            {
                for (int j = i + 1; j < locations.Length; j++)
                {
                    foreach (
                        var antinode in FindAntinodesForPairOfAntennas(locations[i], locations[j])
                    )
                    {
                        antinodes.Add(antinode);
                    }
                }
            }

            return [.. antinodes];
        }

        protected bool IsPointInMap(Point point)
        {
            var (x, y) = point;
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        protected abstract Point[] FindAntinodesForPairOfAntennas(Point antenna1, Point antenna2);
    }
}
