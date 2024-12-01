namespace Aoc2024.Utils
{
    public static class FileOpener
    {
        public static string[][] ReadIntoSplitLines(string filePath, Func<string, string[]>? splitter = null)
        {
            splitter ??= (line) => [line];

            string fileContent = File.ReadAllText(filePath);
            string[] lines = fileContent.Split('\n');
            return lines.Select(line => splitter(line)).ToArray();
        }
    }
}
