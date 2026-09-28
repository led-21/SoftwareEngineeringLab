namespace SoftwareEngineeringLab.Core.Algorithms.Strings;

/// <summary>
/// Classic string manipulation algorithms and sliding window techniques.
/// </summary>
public class StringProblems
{
    /// <summary>
    /// Valid Palindrome: Checks if a string reads the same forwards and backwards after filtering non-alphanumerics.
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public bool IsPalindrome(string s)
    {
        if (string.IsNullOrEmpty(s))
            return true;

        int left = 0, right = s.Length - 1;

        while (left < right)
        {
            while (left < right && !char.IsLetterOrDigit(s[left]))
                left++;
            while (left < right && !char.IsLetterOrDigit(s[right]))
                right--;

            if (char.ToLowerInvariant(s[left]) != char.ToLowerInvariant(s[right]))
                return false;

            left++;
            right--;
        }
        return true;
    }

    /// <summary>
    /// Valid Anagram: Checks if two strings contain the exact same characters with identical frequencies.
    /// Time Complexity: O(n)
    /// Space Complexity: O(1) assuming fixed 26 lowercase English letters
    /// </summary>
    public bool IsAnagram(string s, string t)
    {
        if (s == null || t == null || s.Length != t.Length)
            return false;

        int[] count = new int[26];

        for (int i = 0; i < s.Length; i++)
        {
            count[s[i] - 'a']++;
            count[t[i] - 'a']--;
        }

        for (int i = 0; i < 26; i++)
        {
            if (count[i] != 0)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Longest Substring Without Repeating Characters (Sliding Window technique).
    /// Time Complexity: O(n)
    /// Space Complexity: O(min(m, n)) where m is charset size
    /// </summary>
    public int LengthOfLongestSubstring(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;

        var charSet = new HashSet<char>();
        int left = 0, maxLength = 0;

        for (int right = 0; right < s.Length; right++)
        {
            while (charSet.Contains(s[right]))
            {
                charSet.Remove(s[left]);
                left++;
            }

            charSet.Add(s[right]);
            maxLength = Math.Max(maxLength, right - left + 1);
        }
        return maxLength;
    }

    /// <summary>
    /// Group Anagrams: Groups anagram strings together using sorted string keys.
    /// Time Complexity: O(n * k log k)
    /// Space Complexity: O(n * k)
    /// </summary>
    public IList<IList<string>> GroupAnagrams(string[] strs)
    {
        if (strs == null || strs.Length == 0)
            return new List<IList<string>>();

        var map = new Dictionary<string, List<string>>();

        foreach (string str in strs)
        {
            char[] chars = str.ToCharArray();
            Array.Sort(chars);
            string key = new string(chars);

            if (!map.TryGetValue(key, out var list))
            {
                list = new List<string>();
                map[key] = list;
            }

            list.Add(str);
        }

        return map.Values.Cast<IList<string>>().ToList();
    }
}
