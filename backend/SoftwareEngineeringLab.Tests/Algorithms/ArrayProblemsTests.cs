using SoftwareEngineeringLab.Core.Algorithms.Arrays;
using Xunit;

namespace SoftwareEngineeringLab.Tests.Algorithms;

public class ArrayProblemsTests
{
    private readonly ArrayProblems _sut = new();

    [Fact]
    public void TwoSum_WhenTargetExists_ReturnsCorrectIndices()
    {
        int[] nums = { 2, 7, 11, 15 };
        var result = _sut.TwoSum(nums, 9);
        Assert.Equal(new[] { 0, 1 }, result);
    }

    [Fact]
    public void TwoSum_WhenTargetDoesNotExist_ReturnsEmpty()
    {
        int[] nums = { 1, 2, 3 };
        var result = _sut.TwoSum(nums, 10);
        Assert.Empty(result);
    }

    [Fact]
    public void MaxSubArray_Kadane_ReturnsMaximumSum()
    {
        int[] nums = { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
        int max = _sut.MaxSubArray(nums);
        Assert.Equal(6, max); // [4, -1, 2, 1]
    }

    [Fact]
    public void MaxProfit_ReturnsHighestSingleTransactionGain()
    {
        int[] prices = { 7, 1, 5, 3, 6, 4 };
        int profit = _sut.MaxProfit(prices);
        Assert.Equal(5, profit); // Buy at 1, sell at 6
    }

    [Fact]
    public void Rotate_RotatesElementsInPlace()
    {
        int[] nums = { 1, 2, 3, 4, 5, 6, 7 };
        _sut.Rotate(nums, 3);
        Assert.Equal(new[] { 5, 6, 7, 1, 2, 3, 4 }, nums);
    }

    [Fact]
    public void ContainsDuplicate_IdentifiesDuplicatesAccurately()
    {
        Assert.True(_sut.ContainsDuplicate(new[] { 1, 2, 3, 1 }));
        Assert.False(_sut.ContainsDuplicate(new[] { 1, 2, 3, 4 }));
    }

    [Fact]
    public void ProductExceptSelf_ComputesProductsWithoutDivision()
    {
        int[] nums = { 1, 2, 3, 4 };
        var result = _sut.ProductExceptSelf(nums);
        Assert.Equal(new[] { 24, 12, 8, 6 }, result);
    }
}
