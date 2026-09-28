using SoftwareEngineeringLab.Core.Algorithms.Searching;
using Xunit;

namespace SoftwareEngineeringLab.Tests.Algorithms;

public class SearchingAlgorithmsTests
{
    private readonly SearchingAlgorithms _sut = new();

    [Theory]
    [InlineData(new[] { 1, 3, 5, 7, 9, 11 }, 7, 3)]
    [InlineData(new[] { 1, 3, 5, 7, 9, 11 }, 1, 0)]
    [InlineData(new[] { 1, 3, 5, 7, 9, 11 }, 11, 5)]
    [InlineData(new[] { 1, 3, 5, 7, 9, 11 }, 4, -1)]
    public void BinarySearch_FindsTargetOrReturnsMinusOne(int[] nums, int target, int expected)
    {
        int result = _sut.BinarySearch(nums, target);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BinarySearchTrace_CapturesStepByStepProgression()
    {
        int[] nums = { 2, 4, 6, 8, 10, 12, 14, 16 };
        var trace = _sut.BinarySearchTrace(nums, 12);

        Assert.Equal(5, trace.FoundIndex);
        Assert.NotEmpty(trace.Steps);
        Assert.True(trace.Steps.Count <= 4); // O(log2(8))
    }

    [Theory]
    [InlineData(new[] { 4, 5, 6, 7, 0, 1, 2 }, 0, 4)]
    [InlineData(new[] { 4, 5, 6, 7, 0, 1, 2 }, 3, -1)]
    [InlineData(new[] { 1 }, 0, -1)]
    public void SearchRotated_FindsElementInShiftedArray(int[] nums, int target, int expected)
    {
        int result = _sut.SearchRotated(nums, target);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SearchRange_FindsFirstAndLastOccurrences()
    {
        int[] nums = { 5, 7, 7, 8, 8, 10 };
        var range = _sut.SearchRange(nums, 8);
        Assert.Equal(new[] { 3, 4 }, range);
    }
}
