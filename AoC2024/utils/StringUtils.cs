namespace Aoc2024.Utils
{

    public static class StringUtils
    {
        public static bool CompareOrderInsensitive(string str1, string str2)
        {
            return str1 == str2 || string.Join("", str1.Reverse()) == str2;
        }
    }
}
