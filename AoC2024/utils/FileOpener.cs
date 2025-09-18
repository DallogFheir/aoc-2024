namespace Aoc2024.Utils
{
    public static class FileOpener
    {
        public static T[] ReadIntoSplitLines<T>(string filePath, Func<string, T>? splitter = null)
        {
            splitter ??= (line) => (T)(object)line;

            string fileContent = File.ReadAllText(filePath);
            string[] lines = fileContent.Split('\n');
            return lines.Select((line) => splitter(line)).ToArray();
        }

        public static Grid<T> ReadIntoGrid<T>(string filePath, Func<char, T> converter)
        {
            return new Grid<T>(
                ReadIntoSplitLines(filePath, (line) => line.Select((el) => converter(el)).ToArray())
            );
        }

        public static Grid<char> ReadIntoGrid(string filePath)
        {
            return ReadIntoGrid(filePath, (el) => el);
        }

        public static (string, string) ReadIntoTwoParts(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            string[] parts = fileContent.Split("\n\n");
            return (parts[0], parts[1]);
        }

        public static (T[][], U[][]) ReadIntoTwoPartsThenSplitLines<T, U>(
            string filePath,
            Func<string, T[]>? firstPartSplitter = null,
            Func<string, U[]>? secondPartSplitter = null
        )
        {
            firstPartSplitter ??= (line) => [(T)(object)line];
            secondPartSplitter ??= (line) => [(U)(object)line];

            (string firstPart, string secondPart) = ReadIntoTwoParts(filePath);

            T[][] firstPartLines =
            [
                .. firstPart.Split('\n').Select((line) => firstPartSplitter(line)),
            ];
            U[][] secondPartLines =
            [
                .. secondPart.Split('\n').Select((line) => secondPartSplitter(line)),
            ];

            return (firstPartLines, secondPartLines);
        }
    }
}
