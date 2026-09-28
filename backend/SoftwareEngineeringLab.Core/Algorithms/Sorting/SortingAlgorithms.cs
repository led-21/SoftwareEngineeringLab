namespace SoftwareEngineeringLab.Core.Algorithms.Sorting;

/// <summary>
/// Classic comparison-based sorting algorithms.
/// </summary>
public class SortingAlgorithms
{
    /// <summary>
    /// Quick Sort (Lomuto partition scheme).
    /// Time Complexity: O(n log n) average, O(n^2) worst case
    /// Space Complexity: O(log n) call stack
    /// </summary>
    public void QuickSort(int[] arr)
    {
        if (arr == null || arr.Length <= 1)
            return;

        QuickSortInternal(arr, 0, arr.Length - 1);
    }

    public void QuickSort(int[] arr, int low, int high)
    {
        if (arr == null || arr.Length <= 1)
            return;

        QuickSortInternal(arr, low, high);
    }

    private void QuickSortInternal(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high);
            QuickSortInternal(arr, low, pi - 1);
            QuickSortInternal(arr, pi + 1, high);
        }
    }

    private int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }
        Swap(arr, i + 1, high);
        return i + 1;
    }

    /// <summary>
    /// Merge Sort (Divide and Conquer).
    /// Time Complexity: O(n log n)
    /// Space Complexity: O(n)
    /// </summary>
    public void MergeSort(int[] arr)
    {
        if (arr == null || arr.Length <= 1)
            return;

        MergeSortInternal(arr, 0, arr.Length - 1);
    }

    public void MergeSort(int[] arr, int left, int right)
    {
        if (arr == null || arr.Length <= 1)
            return;

        MergeSortInternal(arr, left, right);
    }

    private void MergeSortInternal(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int mid = left + (right - left) / 2;
            MergeSortInternal(arr, left, mid);
            MergeSortInternal(arr, mid + 1, right);
            Merge(arr, left, mid, right);
        }
    }

    private void Merge(int[] arr, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;

        int[] leftArr = new int[n1];
        int[] rightArr = new int[n2];

        Array.Copy(arr, left, leftArr, 0, n1);
        Array.Copy(arr, mid + 1, rightArr, 0, n2);

        int i = 0, j = 0, k = left;

        while (i < n1 && j < n2)
        {
            if (leftArr[i] <= rightArr[j])
                arr[k++] = leftArr[i++];
            else
                arr[k++] = rightArr[j++];
        }

        while (i < n1) arr[k++] = leftArr[i++];
        while (j < n2) arr[k++] = rightArr[j++];
    }

    /// <summary>
    /// Heap Sort (In-place binary heap).
    /// Time Complexity: O(n log n)
    /// Space Complexity: O(1)
    /// </summary>
    public void HeapSort(int[] arr)
    {
        if (arr == null || arr.Length <= 1)
            return;

        int n = arr.Length;

        // Build max-heap
        for (int i = n / 2 - 1; i >= 0; i--)
            Heapify(arr, n, i);

        // Extract elements one by one
        for (int i = n - 1; i > 0; i--)
        {
            Swap(arr, 0, i);
            Heapify(arr, i, 0);
        }
    }

    private void Heapify(int[] arr, int n, int i)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < n && arr[left] > arr[largest])
            largest = left;

        if (right < n && arr[right] > arr[largest])
            largest = right;

        if (largest != i)
        {
            Swap(arr, i, largest);
            Heapify(arr, n, largest);
        }
    }

    private void Swap(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}
