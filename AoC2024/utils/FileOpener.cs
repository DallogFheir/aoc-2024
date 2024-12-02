namespace Aoc2024.Utils
{
    public static class FileOpener
    {
        public static T[][] ReadIntoSplitLines<T>(string filePath, Func<string, T[]>? splitter = null)
        {
            splitter ??= (line) => [(T)(object)line];

            string fileContent = File.ReadAllText(filePath);
            string[] lines = fileContent.Split('\n');
            return lines.Select(line => splitter(line)).ToArray();
        }
    }
}
