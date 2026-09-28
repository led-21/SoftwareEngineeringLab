using SoftwareEngineeringLab.Core.Algorithms.DynamicProgramming;
using Xunit;

namespace SoftwareEngineeringLab.Tests.Algorithms;

public class DynamicProgrammingProblemsTests
{
    private readonly DynamicProgrammingProblems _sut = new();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(6, 8)]
    [InlineData(10, 55)]
    public void Fibonacci_CalculatesValues(int n, int expected)
    {
        Assert.Equal(expected, _sut.Fibonacci(n));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 5)]
    [InlineData(5, 8)]
    public void ClimbStairs_CalculatesDistinctCombinations(int n, int expected)
    {
        Assert.Equal(expected, _sut.ClimbStairs(n));
    }

    [Fact]
    public void Rob_MaximizesLootWithoutAlertingAlarm()
    {
        int[] houses = { 2, 7, 9, 3, 1 };
        int maxLoot = _sut.Rob(houses);
        Assert.Equal(12, maxLoot); // 2 + 9 + 1 = 12
    }

    [Fact]
    public void CoinChange_FindsMinimumCoinsOrMinusOne()
    {
        int[] coins = { 1, 2, 5 };
        Assert.Equal(3, _sut.CoinChange(coins, 11)); // 5 + 5 + 1
        Assert.Equal(-1, _sut.CoinChange(new[] { 2 }, 3));
    }

    [Fact]
    public void LengthOfLIS_FindsLongestSubsequenceLength()
    {
        int[] nums = { 10, 9, 2, 5, 3, 7, 101, 18 };
        Assert.Equal(4, _sut.LengthOfLIS(nums)); // [2, 3, 7, 101]
    }
}
