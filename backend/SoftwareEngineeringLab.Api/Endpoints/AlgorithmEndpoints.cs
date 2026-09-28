using SoftwareEngineeringLab.Core.Algorithms.Arrays;
using SoftwareEngineeringLab.Core.Algorithms.Graphs;
using SoftwareEngineeringLab.Core.Algorithms.Searching;
using SoftwareEngineeringLab.Core.Algorithms.Sorting;

namespace SoftwareEngineeringLab.Api.Endpoints;

public record BinarySearchRequest(int[] Nums, int Target);
public record SortRequest(int[] Nums, string Algorithm);
public record IslandsRequest(char[][] Grid);

public static class AlgorithmEndpoints
{
    public static void MapAlgorithmEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/algorithms").WithTags("Algorithms");

        var searching = new SearchingAlgorithms();
        var sorting = new SortingAlgorithms();
        var graphs = new GraphProblems();

        group.MapPost("/binary-search", (BinarySearchRequest req) =>
        {
            if (req.Nums == null || req.Nums.Length == 0)
                return Results.BadRequest(new { error = "Input array cannot be empty." });

            var result = searching.BinarySearchTrace(req.Nums, req.Target);

            return Results.Ok(new
            {
                input = req.Nums,
                target = req.Target,
                foundIndex = result.FoundIndex,
                steps = result.Steps,
                timeComplexity = "O(log n)",
                spaceComplexity = "O(1)"
            });
        });

        group.MapPost("/sort", (SortRequest req) =>
        {
            if (req.Nums == null)
                return Results.BadRequest(new { error = "Input array cannot be null." });

            var copy = (int[])req.Nums.Clone();
            string algo = req.Algorithm?.ToLowerInvariant() ?? "quicksort";

            string timeComp = "O(n log n)";
            string spaceComp = "O(log n)";

            if (algo == "mergesort" || algo == "merge")
            {
                sorting.MergeSort(copy);
                algo = "Merge Sort";
                spaceComp = "O(n)";
            }
            else if (algo == "heapsort" || algo == "heap")
            {
                sorting.HeapSort(copy);
                algo = "Heap Sort";
                spaceComp = "O(1)";
            }
            else
            {
                sorting.QuickSort(copy);
                algo = "Quick Sort";
            }

            return Results.Ok(new
            {
                algorithm = algo,
                original = req.Nums,
                sorted = copy,
                timeComplexity = timeComp,
                spaceComplexity = spaceComp
            });
        });

        group.MapPost("/islands", (IslandsRequest req) =>
        {
            if (req.Grid == null || req.Grid.Length == 0)
                return Results.BadRequest(new { error = "Grid cannot be empty." });

            int count = graphs.NumIslands(req.Grid);

            return Results.Ok(new
            {
                islandsCount = count,
                rows = req.Grid.Length,
                columns = req.Grid[0].Length,
                timeComplexity = "O(m * n)",
                spaceComplexity = "O(m * n)"
            });
        });
    }
}
