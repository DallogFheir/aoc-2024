using System.Text.RegularExpressions;

namespace Aoc2024.Day14
{
    public class InputParser(int maxX, int maxY)
    {
        public Robot Parse(string line)
        {
            string numberPattern = @"(-?\d+)";
            string robotPattern =
                $"p={numberPattern},{numberPattern} v={numberPattern},{numberPattern}";

            Match match = Regex.Match(line, robotPattern);
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid input line: {line}");
            }

            int startX = int.Parse(match.Groups[1].Value);
            int startY = int.Parse(match.Groups[2].Value);
            int velocityX = int.Parse(match.Groups[3].Value);
            int velocityY = int.Parse(match.Groups[4].Value);

            return new Robot(new MapPoint(startX, startY, maxX, maxY), velocityX, velocityY);
        }
    }
}
