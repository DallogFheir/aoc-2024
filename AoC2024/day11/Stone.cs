namespace Aoc2024.Day11
{
    public class Stone(long number)
    {
        private readonly long multiplier = 2024;

        public Stone[] Change()
        {
            if (number == 0)
            {
                return [new Stone(1)];
            }

            var numberStr = number.ToString();
            if (numberStr.Length % 2 == 0)
            {
                var left = long.Parse(numberStr[..(numberStr.Length / 2)]);
                var right = long.Parse(numberStr[(numberStr.Length / 2)..]);
                return [new Stone(left), new Stone(right)];
            }

            return [new Stone(number * multiplier)];
        }
    }
}
