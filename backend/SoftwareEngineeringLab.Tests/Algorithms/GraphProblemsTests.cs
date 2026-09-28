using SoftwareEngineeringLab.Core.Algorithms.Graphs;
using Xunit;

namespace SoftwareEngineeringLab.Tests.Algorithms;

public class GraphProblemsTests
{
    private readonly GraphProblems _sut = new();

    [Fact]
    public void DFS_ReturnsTraversalInDepthFirstOrder()
    {
        var graph = new Dictionary<int, List<int>>
        {
            [0] = new() { 1, 2 },
            [1] = new() { 3 },
            [2] = new() { 4 },
            [3] = new(),
            [4] = new()
        };

        var result = _sut.DFS(graph, 0);
        Assert.Equal(5, result.Count);
        Assert.Equal(0, result[0]);
    }

    [Fact]
    public void BFS_ReturnsTraversalInBreadthFirstOrder()
    {
        var graph = new Dictionary<int, List<int>>
        {
            [0] = new() { 1, 2 },
            [1] = new() { 3 },
            [2] = new() { 4 },
            [3] = new(),
            [4] = new()
        };

        var result = _sut.BFS(graph, 0);
        Assert.Equal(new[] { 0, 1, 2, 3, 4 }, result);
    }

    [Fact]
    public void NumIslands_CountsConnectedLandComponents()
    {
        char[][] grid = {
            new[] { '1', '1', '0', '0', '0' },
            new[] { '1', '1', '0', '0', '0' },
            new[] { '0', '0', '1', '0', '0' },
            new[] { '0', '0', '0', '1', '1' }
        };

        int count = _sut.NumIslands(grid);
        Assert.Equal(3, count);
    }

    [Fact]
    public void CanFinish_DetectsValidScheduleAndCycles()
    {
        // 2 courses: [1, 0] means take 0 then 1 -> valid
        Assert.True(_sut.CanFinish(2, new[] { new[] { 1, 0 } }));

        // 2 courses: [1, 0], [0, 1] means mutual dependency -> cyclic
        Assert.False(_sut.CanFinish(2, new[] { new[] { 1, 0 }, new[] { 0, 1 } }));
    }
}
