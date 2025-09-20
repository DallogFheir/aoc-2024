namespace Aoc2024.Utils
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    public static class DirectionUtils
    {
        public static Direction TurnLeft(Direction direction)
        {
            int directionCount = Enum.GetValues(typeof(Direction)).Length;

            int directionInt = (int)direction;
            int newDirectionInt = directionInt == 0 ? directionCount - 1 : directionInt - 1;
            return (Direction)newDirectionInt;
        }

        public static Direction TurnRight(Direction direction)
        {
            int directionCount = Enum.GetValues(typeof(Direction)).Length;
            return (Direction)(((int)direction + 1) % directionCount);
        }

        public static Point GetPointAfterMovement(Point currentPosition, Direction direction)
        {
            var (x, y) = currentPosition;

            return direction switch
            {
                Direction.Up => (x, y - 1),
                Direction.Right => (x + 1, y),
                Direction.Down => (x, y + 1),
                Direction.Left => (x - 1, y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
            };
        }
    }
}
