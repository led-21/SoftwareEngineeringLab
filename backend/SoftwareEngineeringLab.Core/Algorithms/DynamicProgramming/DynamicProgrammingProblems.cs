namespace SoftwareEngineeringLab.Core.Algorithms.DynamicProgramming;

/// <summary>
/// Classic Dynamic Programming problems with optimal sub-structure and overlapping sub-problems.
/// </summary>
public class DynamicProgrammingProblems
{
    /// <summary>
    /// Fibonacci with space-optimized state transition.
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public int Fibonacci(int n)
    {
        if (n <= 1)
            return Math.Max(0, n);

        int prev2 = 0, prev1 = 1;
        for (int i = 2; i <= n; i++)
        {
            int current = prev1 + prev2;
            prev2 = prev1;
            prev1 = current;
        }
        return prev1;
    }

    /// <summary>
    /// Climbing Stairs (number of distinct ways to climb n steps taking 1 or 2 steps).
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public int ClimbStairs(int n)
    {
        if (n <= 2)
            return Math.Max(0, n);

        int prev2 = 1, prev1 = 2;
        for (int i = 3; i <= n; i++)
        {
            int current = prev1 + prev2;
            prev2 = prev1;
            prev1 = current;
        }
        return prev1;
    }

    /// <summary>
    /// House Robber (Maximum amount without robbing adjacent houses).
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public int Rob(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return 0;
        if (nums.Length == 1)
            return nums[0];

        int prev2 = nums[0];
        int prev1 = Math.Max(nums[0], nums[1]);

        for (int i = 2; i < nums.Length; i++)
        {
            int current = Math.Max(prev1, prev2 + nums[i]);
            prev2 = prev1;
            prev1 = current;
        }
        return prev1;
    }

    /// <summary>
    /// Coin Change (Fewest number of coins needed to make up a given amount).
    /// Time Complexity: O(amount * coins.length)
    /// Space Complexity: O(amount)
    /// </summary>
    public int CoinChange(int[] coins, int amount)
    {
        if (amount < 0)
            return -1;
        if (amount == 0)
            return 0;
        if (coins == null || coins.Length == 0)
            return -1;

        int[] dp = new int[amount + 1];
        Array.Fill(dp, amount + 1);
        dp[0] = 0;

        for (int i = 1; i <= amount; i++)
        {
            foreach (int coin in coins)
            {
                if (coin <= i)
                    dp[i] = Math.Min(dp[i], dp[i - coin] + 1);
            }
        }
        return dp[amount] > amount ? -1 : dp[amount];
    }

    /// <summary>
    /// Longest Increasing Subsequence (LIS).
    /// Time Complexity: O(n^2)
    /// Space Complexity: O(n)
    /// </summary>
    public int LengthOfLIS(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return 0;

        int[] dp = new int[nums.Length];
        Array.Fill(dp, 1);
        int maxLength = 1;

        for (int i = 1; i < nums.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (nums[i] > nums[j])
                    dp[i] = Math.Max(dp[i], dp[j] + 1);
            }
            maxLength = Math.Max(maxLength, dp[i]);
        }
        return maxLength;
    }
}
