using System.Diagnostics;

namespace Aoc2024.Utils
{
    public static class Assert
    {
        public static void ExpectedEqualsActual(object expected, object actual)
        {
            Debug.Assert(expected.Equals(actual), $"expected {actual} to equal {expected}");
        }
    }
}
