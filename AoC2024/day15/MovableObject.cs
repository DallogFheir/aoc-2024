namespace Aoc2024.Day15;

using Aoc2024.Utils;

public class MovableObject(Point leftmostPoint, int width)
{
    public Point LeftmostPoint { get; private set; } = leftmostPoint;
    public int Width { get; } = width;

    public override bool Equals(object? obj)
    {
        if (obj is MovableObject otherMovableObject)
        {
            return LeftmostPoint == otherMovableObject.LeftmostPoint
                && Width == otherMovableObject.Width;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LeftmostPoint, Width);
    }

    public int GetGpsCoordinate()
    {
        var (x, y) = LeftmostPoint;
        return x + 100 * y;
    }

    public Point[] GetPositions()
    {
        return
        [
            .. Enumerable
                .Range(0, Width)
                .Select(offset => (LeftmostPoint.Item1 + offset, LeftmostPoint.Item2)),
        ];
    }

    public Point[] GetPositionsAfterMovement(Direction direction)
    {
        var leftmostPointAfterMovement = DirectionUtils.GetPointAfterMovement(
            LeftmostPoint,
            direction
        );

        return
        [
            .. Enumerable
                .Range(0, Width)
                .Select(offset =>
                    (leftmostPointAfterMovement.Item1 + offset, leftmostPointAfterMovement.Item2)
                ),
        ];
    }

    public void Move(Direction direction)
    {
        LeftmostPoint = GetPositionsAfterMovement(direction)[0];
    }
}
