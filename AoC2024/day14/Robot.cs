namespace Aoc2024.Day14
{
    public class Robot(MapPoint startingPosition, int velocityX, int velocityY)
    {
        public MapPoint Position { get; private set; } = startingPosition;

        public void Move(int howManyTimes)
        {
            Position = Position.GetAfterMove(velocityX * howManyTimes, velocityY * howManyTimes);
        }

        public Quadrant GetQuadrant()
        {
            return Position.GetQuadrant();
        }
    }
}
