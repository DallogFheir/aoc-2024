namespace Aoc2024.Day07
{
    using Expression = (long, long[]);
    using Operator = Func<long, long, long>;

    public class ExpressionValidator(Operator[] _operators)
    {
        private readonly Operator[] operators = _operators;

        public bool IsExpressionValid(Expression expression)
        {
            var (result, values) = expression;

            return IsExpressionValid(result, 0, values);
        }

        private bool IsExpressionValid(long result, long accumulator, long[] values)
        {
            if (values.Length == 0)
            {
                return result == accumulator;
            }

            if (accumulator > result)
            {
                return false;
            }

            var nextValue = values[0];
            var remainingValues = values.Skip(1).ToArray();

            return operators.Any(
                (op) => IsExpressionValid(result, op(accumulator, nextValue), remainingValues)
            );
        }
    }
}
