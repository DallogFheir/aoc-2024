namespace Aoc2024.Utils
{
    public class Graph<T>
        where T : notnull
    {
        private readonly Dictionary<T, HashSet<T>> adjacencyList;

        public Graph()
        {
            adjacencyList = [];
        }

        public Graph((T, T)[] edges)
            : this()
        {
            Array.ForEach(
                edges,
                (edge) =>
                {
                    var (source, neighbor) = edge;
                    AddEdge(source, neighbor);
                }
            );
        }

        public void AddEdge(T source, T neighbor)
        {
            var sourceNeighbors = adjacencyList.GetValueOrDefault(source, []);
            sourceNeighbors.Add(neighbor);
            adjacencyList[source] = sourceNeighbors;

            if (!adjacencyList.ContainsKey(neighbor))
            {
                adjacencyList[neighbor] = [];
            }
        }

        public T[] SortTopologically()
        {
            HashSet<T> visited = [];
            Stack<T> stack = [];

            void visit(T node)
            {
                if (!visited.Contains(node))
                {
                    visited.Add(node);

                    foreach (var neighbor in adjacencyList[node])
                    {
                        visit(neighbor);
                    }

                    stack.Push(node);
                }
            }

            foreach (var node in adjacencyList.Keys)
            {
                visit(node);
            }

            return [.. stack];
        }
    }
}
