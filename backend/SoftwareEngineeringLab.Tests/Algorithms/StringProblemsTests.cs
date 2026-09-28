using SoftwareEngineeringLab.Core.Algorithms.Strings;
using Xunit;

namespace SoftwareEngineeringLab.Tests.Algorithms;

public class StringProblemsTests
{
    private readonly StringProblems _sut = new();

    [Theory]
    [InlineData("A man, a plan, a canal: Panama", true)]
    [InlineData("race a car", false)]
    [InlineData(" ", true)]
    public void IsPalindrome_ChecksAlphanumerics(string s, bool expected)
    {
        Assert.Equal(expected, _sut.IsPalindrome(s));
    }

    [Theory]
    [InlineData("anagram", "nagaram", true)]
    [InlineData("rat", "car", false)]
    public void IsAnagram_ChecksFrequency(string s, string t, bool expected)
    {
        Assert.Equal(expected, _sut.IsAnagram(s, t));
    }

    [Theory]
    [InlineData("abcabcbb", 3)]
    [InlineData("bbbbb", 1)]
    [InlineData("pwwkew", 3)]
    public void LengthOfLongestSubstring_ComputesWindow(string s, int expected)
    {
        Assert.Equal(expected, _sut.LengthOfLongestSubstring(s));
    }

    [Fact]
    public void GroupAnagrams_GroupsSimilarWords()
    {
        string[] words = { "eat", "tea", "tan", "ate", "nat", "bat" };
        var grouped = _sut.GroupAnagrams(words);

        Assert.Equal(3, grouped.Count);
    }
}
