using Aoc2024.Utils;

namespace Aoc2024.Day09
{
    public class ChecksumCalculator
    {
        public long Checksum { get; protected set; } = 0;

        public void AddBlockToChecksum(int blockId, int blockSize, int position)
        {
            ArithmeticSequence arithmeticSequence = new(position, 1);
            Checksum += (long)(arithmeticSequence.GetSumOfNTerms(blockSize) * blockId);
        }
    }
}
