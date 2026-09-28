namespace SoftwareEngineeringLab.Core.Algorithms.Graphs;

/// <summary>
/// Graph traversal and graph-based problem solving.
/// </summary>
public class GraphProblems
{
    /// <summary>
    /// Depth-First Search (DFS) returning nodes in discovery order.
    /// Time Complexity: O(V + E)
    /// Space Complexity: O(V)
    /// </summary>
    public List<int> DFS(Dictionary<int, List<int>> graph, int start)
    {
        var visited = new HashSet<int>();
        var result = new List<int>();
        DfsHelper(graph, start, visited, result);
        return result;
    }

    private void DfsHelper(Dictionary<int, List<int>> graph, int current, HashSet<int> visited, List<int> result)
    {
        visited.Add(current);
        result.Add(current);

        if (graph.TryGetValue(current, out var neighbors))
        {
            foreach (int neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                    DfsHelper(graph, neighbor, visited, result);
            }
        }
    }

    /// <summary>
    /// Breadth-First Search (BFS) returning nodes in level order.
    /// Time Complexity: O(V + E)
    /// Space Complexity: O(V)
    /// </summary>
    public List<int> BFS(Dictionary<int, List<int>> graph, int start)
    {
        var visited = new HashSet<int>();
        var queue = new Queue<int>();
        var result = new List<int>();

        visited.Add(start);
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            result.Add(node);

            if (graph.TryGetValue(node, out var neighbors))
            {
                foreach (int neighbor in neighbors)
                {
                    if (visited.Add(neighbor))
                        queue.Enqueue(neighbor);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Number of Islands: Counts disconnected land components in a binary grid.
    /// Time Complexity: O(m * n)
    /// Space Complexity: O(m * n)
    /// </summary>
    public int NumIslands(char[][] grid)
    {
        if (grid == null || grid.Length == 0)
            return 0;

        int numIslands = 0;
        int rows = grid.Length;
        int cols = grid[0].Length;

        // Clone grid to preserve original input
        char[][] copy = new char[rows][];
        for (int i = 0; i < rows; i++)
            copy[i] = (char[])grid[i].Clone();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (copy[i][j] == '1')
                {
                    numIslands++;
                    DfsIsland(copy, i, j);
                }
            }
        }
        return numIslands;
    }

    private void DfsIsland(char[][] grid, int r, int c)
    {
        if (r < 0 || r >= grid.Length || c < 0 || c >= grid[0].Length || grid[r][c] != '1')
            return;

        grid[r][c] = '0'; // Sink the visited land

        DfsIsland(grid, r + 1, c);
        DfsIsland(grid, r - 1, c);
        DfsIsland(grid, r, c + 1);
        DfsIsland(grid, r, c - 1);
    }

    /// <summary>
    /// Course Schedule: Detect cycles in directed graph using Kahn's Algorithm (Topological Sort).
    /// Time Complexity: O(V + E)
    /// Space Complexity: O(V + E)
    /// </summary>
    public bool CanFinish(int numCourses, int[][] prerequisites)
    {
        if (numCourses <= 1 || prerequisites == null || prerequisites.Length == 0)
            return true;

        var graph = new Dictionary<int, List<int>>();
        var inDegree = new int[numCourses];

        foreach (var prereq in prerequisites)
        {
            int course = prereq[0];
            int prerequisite = prereq[1];

            if (!graph.ContainsKey(prerequisite))
                graph[prerequisite] = new List<int>();

            graph[prerequisite].Add(course);
            inDegree[course]++;
        }

        var queue = new Queue<int>();
        for (int i = 0; i < numCourses; i++)
        {
            if (inDegree[i] == 0)
                queue.Enqueue(i);
        }

        int processed = 0;
        while (queue.Count > 0)
        {
            int course = queue.Dequeue();
            processed++;

            if (graph.TryGetValue(course, out var neighbors))
            {
                foreach (int neighbor in neighbors)
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0)
                        queue.Enqueue(neighbor);
                }
            }
        }

        return processed == numCourses;
    }
}
