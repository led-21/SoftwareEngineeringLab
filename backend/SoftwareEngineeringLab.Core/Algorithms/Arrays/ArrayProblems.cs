namespace SoftwareEngineeringLab.Core.Algorithms.Arrays;

/// <summary>
/// Classic array algorithmic interview problems.
/// </summary>
public class ArrayProblems
{
    /// <summary>
    /// Two Sum: Finds indices of two numbers that add up to target.
    /// Time Complexity: O(n)
    /// Space Complexity: O(n)
    /// </summary>
    public int[] TwoSum(int[] nums, int target)
    {
        var map = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];
            if (map.TryGetValue(complement, out int complementIndex))
                return new[] { complementIndex, i };

            map[nums[i]] = i;
        }
        return Array.Empty<int>();
    }

    /// <summary>
    /// Maximum Subarray (Kadane's Algorithm).
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public int MaxSubArray(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            throw new ArgumentException("Array cannot be null or empty.", nameof(nums));

        int maxSoFar = nums[0];
        int maxEndingHere = nums[0];

        for (int i = 1; i < nums.Length; i++)
        {
            maxEndingHere = Math.Max(nums[i], maxEndingHere + nums[i]);
            maxSoFar = Math.Max(maxSoFar, maxEndingHere);
        }
        return maxSoFar;
    }

    /// <summary>
    /// Best Time to Buy and Sell Stock: Maximize profit with a single buy and single sell.
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public int MaxProfit(int[] prices)
    {
        if (prices == null || prices.Length <= 1)
            return 0;

        int minPrice = int.MaxValue;
        int maxProfit = 0;

        foreach (int price in prices)
        {
            if (price < minPrice)
                minPrice = price;
            else if (price - minPrice > maxProfit)
                maxProfit = price - minPrice;
        }
        return maxProfit;
    }

    /// <summary>
    /// Rotate Array: Rotates an array to the right by k steps.
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public void Rotate(int[] nums, int k)
    {
        if (nums == null || nums.Length <= 1)
            return;

        k %= nums.Length;
        if (k < 0)
            k += nums.Length;

        if (k == 0)
            return;

        Reverse(nums, 0, nums.Length - 1);
        Reverse(nums, 0, k - 1);
        Reverse(nums, k, nums.Length - 1);
    }

    private void Reverse(int[] nums, int start, int end)
    {
        while (start < end)
        {
            int temp = nums[start];
            nums[start] = nums[end];
            nums[end] = temp;
            start++;
            end--;
        }
    }

    /// <summary>
    /// Contains Duplicate: Checks whether any value appears at least twice.
    /// Time Complexity: O(n)
    /// Space Complexity: O(n)
    /// </summary>
    public bool ContainsDuplicate(int[] nums)
    {
        if (nums == null || nums.Length <= 1)
            return false;

        var seen = new HashSet<int>(nums.Length);
        foreach (int num in nums)
        {
            if (!seen.Add(num))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Product of Array Except Self: Computes prefix and suffix products without division.
    /// Time Complexity: O(n)
    /// Space Complexity: O(1) extra space (excluding output array)
    /// </summary>
    public int[] ProductExceptSelf(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return Array.Empty<int>();

        int[] result = new int[nums.Length];

        // Left prefix products
        result[0] = 1;
        for (int i = 1; i < nums.Length; i++)
            result[i] = result[i - 1] * nums[i - 1];

        // Right suffix products
        int rightProduct = 1;
        for (int i = nums.Length - 1; i >= 0; i--)
        {
            result[i] *= rightProduct;
            rightProduct *= nums[i];
        }

        return result;
    }
}
