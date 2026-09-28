# Data Structures & Algorithms (DSA) Study Notes

This section documents the computational complexity, invariants, and implementation patterns for the algorithmic catalog in `SoftwareEngineeringLab.Core.Algorithms`.

---

## 1. Array & Sliding Window Techniques

### Kadane's Algorithm (Maximum Subarray)
- **Problem**: Find the contiguous subarray with the largest sum.
- **Invariant**: At each position $i$, the maximum sum ending at $i$ is $\max(nums[i], \text{maxEndingHere} + nums[i])$.
- **Complexity**: Time: $O(n)$, Space: $O(1)$.

### Product of Array Except Self
- **Problem**: Return array where each element is the product of all elements except itself, without division.
- **Approach**: Compute prefix products from the left, then accumulate suffix products from the right.
- **Complexity**: Time: $O(n)$, Space: $O(1)$ extra memory.

---

## 2. Searching & Two Pointers

### Binary Search
- **Condition**: Monotonically sorted array.
- **Pointers**: `left = 0`, `right = n - 1`. `mid = left + (right - left) / 2` (prevents integer 32-bit overflow).
- **Complexity**: Time: $O(\log n)$, Space: $O(1)$.

### Search in Rotated Sorted Array
- **Key Insight**: In any circularly rotated sorted array, dividing the array at the midpoint yields at least one half that is guaranteed to be strictly sorted.
- Check which half is sorted, then determine whether the target lies within that sorted range.

---

## 3. Sorting Algorithms

| Algorithm | Average Time | Worst Time | Space | Stability | Mechanism |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **QuickSort** | $O(n \log n)$ | $O(n^2)$ | $O(\log n)$ | No | Lomuto partition around pivot |
| **MergeSort** | $O(n \log n)$ | $O(n \log n)$ | $O(n)$ | Yes | Divide, conquer & merge auxiliary buffers |
| **HeapSort** | $O(n \log n)$ | $O(n \log n)$ | $O(1)$ | No | Max-heap heapify in-place |

---

## 4. Graph Algorithms

### Connected Components (Number of Islands)
- **Graph Model**: An $M \times N$ matrix treated as an undirected graph where cells with `'1'` have edges to their 4 orthogonal neighbors.
- **Traversal**: Sink visited cells (`'1' -> '0'`) via DFS or BFS recursion.
- **Complexity**: Time: $O(M \times N)$, Space: $O(M \times N)$ call stack.

### Topological Sort (Course Schedule)
- **Kahn's Algorithm**:
  1. Build adjacency list and compute in-degrees for every vertex.
  2. Enqueue all vertices with in-degree 0.
  3. Dequeue vertex, decrement neighbor in-degrees, and enqueue newly zeroed neighbors.
  4. If processed count equals total vertices, graph is an acyclic DAG; otherwise, a cycle exists.
- **Complexity**: Time: $O(V + E)$, Space: $O(V + E)$.
