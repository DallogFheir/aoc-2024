namespace Aoc2024.Day05
{
    public class InstructionChecker(int[] instruction, Dictionary<int, HashSet<int>> rules)
    {
        private readonly HashSet<int> alreadySeenPages = [];

        public bool IsInstructionValid()
        {
            return Array.TrueForAll(instruction, (page) => IsPageValid(page));
        }

        private bool IsPageValid(int page)
        {
            var pagesAfter = rules.GetValueOrDefault(page, []);

            foreach (var alreadySeenPage in alreadySeenPages)
            {
                if (pagesAfter.Contains(alreadySeenPage))
                {
                    return false;
                }
            }

            alreadySeenPages.Add(page);
            return true;
        }
    }
}
