namespace Aoc2024.Utils
{
    public static class FileOpener
    {
        public static T[][] ReadIntoSplitLines<T>(
            string filePath,
            Func<string, T[]>? splitter = null
        )
        {
            splitter ??= (line) => [(T)(object)line];

            string fileContent = File.ReadAllText(filePath);
            string[] lines = fileContent.Split('\n');
            return lines.Select((line) => splitter(line)).ToArray();
        }

        public static Grid<T> ReadIntoGrid<T>(string filePath, Func<char, T>? converter = null)
        {
            converter ??= (el) => (T)(object)el;

            return new Grid<T>(
                ReadIntoSplitLines(filePath, (line) => line.Select((el) => converter(el)).ToArray())
            );
        }

        public static (T[][], U[][]) ReadIntoTwoPartsThenSplitLines<T, U>(
            string filePath,
            Func<string, T[]>? firstPartSplitter = null,
            Func<string, U[]>? secondPartSplitter = null
        )
        {
            firstPartSplitter ??= (line) => [(T)(object)line];
            secondPartSplitter ??= (line) => [(U)(object)line];

            string fileContent = File.ReadAllText(filePath);
            string[] parts = fileContent.Split("\n\n");

            T[][] firstPartLines = parts[0]
                .Split('\n')
                .Select((line) => firstPartSplitter(line))
                .ToArray();
            U[][] secondPartLines = parts[1]
                .Split('\n')
                .Select((line) => secondPartSplitter(line))
                .ToArray();

            return (firstPartLines, secondPartLines);
        }
    }
}
