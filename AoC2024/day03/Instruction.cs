namespace Aoc2024.Day03
{
    public class InstructionExecutor((string, int[])[] instructions)
    {
        private bool areOperationsEnabled = true;

        public int Execute()
        {
            return instructions.Sum(ExecuteInstruction);
        }

        private int ExecuteInstruction((string, int[]) instruction)
        {
            var (name, arguments) = instruction;

            if (name == "do")
            {
                areOperationsEnabled = true;
                return 0;
            }

            if (!areOperationsEnabled)
            {
                return 0;
            }

            switch (name)
            {
                case "mul":
                    return arguments[0] * arguments[1];
                case "don't":
                    areOperationsEnabled = false;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid instruction: {name}");
            }

            return 0;
        }
    }
}
