namespace Aoc2024.Day14
{
    public class MapPoint(int x, int y, int xMax, int yMax)
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public MapPoint GetAfterMove(int dx, int dy)
        {
            var moduloX = xMax + 1;
            var newX = ((X + dx) % moduloX + moduloX) % moduloX;

            var moduloY = yMax + 1;
            var newY = ((Y + dy) % moduloY + moduloY) % moduloY;

            return new MapPoint(newX, newY, xMax, yMax);
        }

        public Quadrant GetQuadrant()
        {
            int middleLineX = xMax / 2;
            int middleLineY = yMax / 2;

            bool isInMiddleX = X == middleLineX;
            bool isInMiddleY = Y == middleLineY;

            if (isInMiddleX || isInMiddleY)
            {
                return Quadrant.None;
            }

            bool isInRight = X > middleLineX;
            bool isInTop = Y > middleLineY;

            switch ((isInRight, isInTop))
            {
                case (true, true):
                    return Quadrant.First;
                case (false, true):
                    return Quadrant.Second;
                case (false, false):
                    return Quadrant.Third;
                case (true, false):
                    return Quadrant.Fourth;
            }
        }
    }
}
