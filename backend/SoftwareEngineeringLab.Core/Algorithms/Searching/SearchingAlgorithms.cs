namespace SoftwareEngineeringLab.Core.Algorithms.Searching;

public record BinarySearchStep(int StepNumber, int Left, int Mid, int Right, int MidValue, string Action);

public record BinarySearchResult(int FoundIndex, List<BinarySearchStep> Steps);

/// <summary>
/// Classic search algorithms.
/// </summary>
public class SearchingAlgorithms
{
    /// <summary>
    /// Binary Search on a sorted array.
    /// Time Complexity: O(log n)
    /// Space Complexity: O(1)
    /// </summary>
    public int BinarySearch(int[] nums, int target)
    {
        if (nums == null || nums.Length == 0)
            return -1;

        int left = 0, right = nums.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (nums[mid] == target)
                return mid;
            if (nums[mid] < target)
                left = mid + 1;
            else
                right = mid - 1;
        }
        return -1;
    }

    /// <summary>
    /// Binary Search that records each inspection step for educational visualization.
    /// </summary>
    public BinarySearchResult BinarySearchTrace(int[] nums, int target)
    {
        var steps = new List<BinarySearchStep>();
        if (nums == null || nums.Length == 0)
            return new BinarySearchResult(-1, steps);

        int left = 0, right = nums.Length - 1;
        int stepCount = 0;

        while (left <= right)
        {
            stepCount++;
            int mid = left + (right - left) / 2;
            int midVal = nums[mid];

            if (midVal == target)
            {
                steps.Add(new BinarySearchStep(stepCount, left, mid, right, midVal, $"Target {target} found at index {mid}"));
                return new BinarySearchResult(mid, steps);
            }

            if (midVal < target)
            {
                steps.Add(new BinarySearchStep(stepCount, left, mid, right, midVal, $"{midVal} < {target}. Discarding left half [{left}..{mid}]"));
                left = mid + 1;
            }
            else
            {
                steps.Add(new BinarySearchStep(stepCount, left, mid, right, midVal, $"{midVal} > {target}. Discarding right half [{mid}..{right}]"));
                right = mid - 1;
            }
        }

        steps.Add(new BinarySearchStep(stepCount + 1, left, -1, right, -1, $"Target {target} not found in array"));
        return new BinarySearchResult(-1, steps);
    }

    /// <summary>
    /// Search in Rotated Sorted Array.
    /// Time Complexity: O(log n)
    /// Space Complexity: O(1)
    /// </summary>
    public int SearchRotated(int[] nums, int target)
    {
        if (nums == null || nums.Length == 0)
            return -1;

        int left = 0, right = nums.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (nums[mid] == target)
                return mid;

            if (nums[left] <= nums[mid]) // Left half is sorted
            {
                if (target >= nums[left] && target < nums[mid])
                    right = mid - 1;
                else
                    left = mid + 1;
            }
            else // Right half is sorted
            {
                if (target > nums[mid] && target <= nums[right])
                    left = mid + 1;
                else
                    right = mid - 1;
            }
        }
        return -1;
    }

    /// <summary>
    /// Find First and Last Position of Element in Sorted Array.
    /// Time Complexity: O(log n)
    /// Space Complexity: O(1)
    /// </summary>
    public int[] SearchRange(int[] nums, int target)
    {
        if (nums == null || nums.Length == 0)
            return new[] { -1, -1 };

        return new[] { FindBound(nums, target, isFirst: true), FindBound(nums, target, isFirst: false) };
    }

    private int FindBound(int[] nums, int target, bool isFirst)
    {
        int left = 0, right = nums.Length - 1;
        int result = -1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (nums[mid] == target)
            {
                result = mid;
                if (isFirst)
                    right = mid - 1; // Narrow down leftward
                else
                    left = mid + 1;  // Narrow down rightward
            }
            else if (nums[mid] < target)
                left = mid + 1;
            else
                right = mid - 1;
        }
        return result;
    }
}
