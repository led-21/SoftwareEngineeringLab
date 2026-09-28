using SoftwareEngineeringLab.Core.Algorithms.Sorting;
using Xunit;

namespace SoftwareEngineeringLab.Tests.Algorithms;

public class SortingAlgorithmsTests
{
    private readonly SortingAlgorithms _sut = new();

    public static IEnumerable<object[]> GetSampleArrays()
    {
        yield return new object[] { new int[] { } };
        yield return new object[] { new[] { 42 } };
        yield return new object[] { new[] { 5, 2, 9, 1, 5, 6 } };
        yield return new object[] { new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 } };
        yield return new object[] { new[] { 1, 2, 3, 4, 5, 6 } };
    }

    [Theory]
    [MemberData(nameof(GetSampleArrays))]
    public void QuickSort_SortsArrayCorrectly(int[] input)
    {
        var arr = (int[])input.Clone();
        var expected = (int[])input.Clone();
        Array.Sort(expected);

        _sut.QuickSort(arr);

        Assert.Equal(expected, arr);
    }

    [Theory]
    [MemberData(nameof(GetSampleArrays))]
    public void MergeSort_SortsArrayCorrectly(int[] input)
    {
        var arr = (int[])input.Clone();
        var expected = (int[])input.Clone();
        Array.Sort(expected);

        _sut.MergeSort(arr);

        Assert.Equal(expected, arr);
    }

    [Theory]
    [MemberData(nameof(GetSampleArrays))]
    public void HeapSort_SortsArrayCorrectly(int[] input)
    {
        var arr = (int[])input.Clone();
        var expected = (int[])input.Clone();
        Array.Sort(expected);

        _sut.HeapSort(arr);

        Assert.Equal(expected, arr);
    }
}
