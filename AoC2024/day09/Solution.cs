using Aoc2024.Utils;

namespace Aoc2024.Day09
{
    using Block = (BlockType type, int blockSize, int? id);

    class IndexedBlock(BlockType type, int blockSize, int? id, int idx)
    {
        public BlockType Type { get; } = type;
        public int BlockSize { get; set; } = blockSize;
        public int? Id { get; } = id;
        public int Idx { get; set; } = idx;
    }

    enum BlockType
    {
        EmptySpace,
        File,
    }

    public class Solution
    {
        public static void Part1()
        {
            var testResult = SolvePart1("test.txt");
            Assert.ExpectedEqualsActual(1928L, testResult);

            var result = SolvePart1("input.txt");
            Console.WriteLine($"Part 1: {result}");
        }

        private static long SolvePart1(string inputPath)
        {
            return Solve(inputPath, CalculateFragmentedFilesystemChecksum);
        }

        private static long Solve(string inputPath, Func<string, long> solver)
        {
            var input = FileOpener.ReadIntoSplitLines<string>($"day09/{inputPath}")[0];
            return solver(input);
        }

        private static long CalculateFragmentedFilesystemChecksum(string diskMap)
        {
            LinkedList<Block> blocks = new(ConvertDiskMapToBlockArray(diskMap));

            AutomaticChecksumCalculator checksumCalculator = new();
            while (blocks.Count > 0)
            {
                var block = blocks.First();
                blocks.RemoveFirst();

                if (block.type == BlockType.File)
                {
                    checksumCalculator.AddBlockToChecksum((int)block.id!, block.blockSize);
                    continue;
                }

                int emptySpaceToFill = block.blockSize;
                while (emptySpaceToFill > 0)
                {
                    Block? maybeLastBlock;
                    do
                    {
                        if (blocks.Count == 0)
                        {
                            return checksumCalculator.Checksum;
                        }

                        maybeLastBlock = blocks.Last();
                        blocks.RemoveLast();
                    } while (maybeLastBlock?.type == BlockType.EmptySpace);

                    Block lastBlock = (Block)maybeLastBlock!;
                    if (lastBlock.blockSize > emptySpaceToFill)
                    {
                        checksumCalculator.AddBlockToChecksum((int)lastBlock.id!, emptySpaceToFill);
                        lastBlock.blockSize -= emptySpaceToFill;
                        emptySpaceToFill = 0;
                        blocks.AddLast(lastBlock);
                        continue;
                    }

                    emptySpaceToFill -= lastBlock.blockSize;
                    checksumCalculator.AddBlockToChecksum((int)lastBlock.id!, lastBlock.blockSize);
                }
            }

            return checksumCalculator.Checksum;
        }

        private static IEnumerable<Block> ConvertDiskMapToBlockArray(string diskMap)
        {
            int id = 0;

            return diskMap.Select(
                (block, idx) =>
                {
                    bool isFile = idx % 2 == 0;

                    Block result = (
                        type: isFile ? BlockType.File : BlockType.EmptySpace,
                        blockSize: (int)char.GetNumericValue(block),
                        id: isFile ? id : null
                    );

                    if (isFile)
                    {
                        id++;
                    }

                    return result;
                }
            );
        }

        public static void Part2()
        {
            var testResult = SolvePart2("test.txt");
            Assert.ExpectedEqualsActual(2858L, testResult);

            var result = SolvePart2("input.txt");
            Console.WriteLine($"Part 2: {result}");
        }

        private static long SolvePart2(string inputPath)
        {
            return Solve(inputPath, CalculateUnfragmentedFilesystemChecksum);
        }

        private static long CalculateUnfragmentedFilesystemChecksum(string diskMap)
        {
            var indexedBlocks = ConvertDiskMapToIndexedBlockArray(diskMap).ToArray();

            var emptySpaces = indexedBlocks.Where((block) => block.Type == BlockType.EmptySpace);
            var reversedFiles = indexedBlocks
                .Where((block) => block.Type == BlockType.File)
                .Reverse();

            ChecksumCalculator checksumCalculator = new();
            foreach (var file in reversedFiles)
            {
                try
                {
                    var blockWithEnoughSpace = emptySpaces.First(
                        (emptySpace) =>
                            emptySpace.Idx < file.Idx && emptySpace.BlockSize >= file.BlockSize
                    );
                    blockWithEnoughSpace.BlockSize -= file.BlockSize;
                    checksumCalculator.AddBlockToChecksum(
                        (int)file.Id!,
                        file.BlockSize,
                        blockWithEnoughSpace.Idx
                    );
                    blockWithEnoughSpace.Idx += file.BlockSize;
                }
                catch (InvalidOperationException)
                {
                    checksumCalculator.AddBlockToChecksum((int)file.Id!, file.BlockSize, file.Idx);
                }
            }

            return checksumCalculator.Checksum;
        }

        private static IEnumerable<IndexedBlock> ConvertDiskMapToIndexedBlockArray(string diskMap)
        {
            var idx = 0;

            return ConvertDiskMapToBlockArray(diskMap)
                .Select(
                    (block) =>
                    {
                        var (type, blockSize, id) = block;
                        var result = new IndexedBlock(type, blockSize, id, idx);

                        idx += blockSize;

                        return result;
                    }
                );
        }
    }
}
