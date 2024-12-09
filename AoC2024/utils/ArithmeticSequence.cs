namespace Aoc2024.Utils
{
    public class ArithmeticSequence(double startTerm, double difference)
    {
        public double GetNthTerm(int n)
        {
            CheckNPositive(n);
            return startTerm + difference * (n - 1);
        }

        private void CheckNPositive(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentException("n must be positive.");
            }
        }

        public double GetSumOfNTerms(int n)
        {
            CheckNPositive(n);
            var nthTerm = GetNthTerm(n);
            return (startTerm + nthTerm) / 2 * n;
        }
    }
}
