namespace Aoc2024.Day14
{
    public class Robot(MapPoint startingPosition, int velocityX, int velocityY)
    {
        private MapPoint position = startingPosition;

        public void Move(int howManyTimes)
        {
            position = position.GetAfterMove(velocityX * howManyTimes, velocityY * howManyTimes);
        }

        public Quadrant GetQuadrant()
        {
            return position.GetQuadrant();
        }
    }
}
