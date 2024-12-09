using Aoc2024.Utils;

namespace Aoc2024.Day09
{
    public class AutomaticChecksumCalculator : ChecksumCalculator
    {
        private int position = 0;

        public void AddBlockToChecksum(int blockId, int blockSize)
        {
            ArithmeticSequence arithmeticSequence = new(position, 1);
            Checksum += (long)(arithmeticSequence.GetSumOfNTerms(blockSize) * blockId);
            position += blockSize;
        }
    }
}
