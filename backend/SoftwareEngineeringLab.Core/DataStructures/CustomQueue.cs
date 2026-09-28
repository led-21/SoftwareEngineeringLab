namespace SoftwareEngineeringLab.Core.DataStructures;

/// <summary>
/// Educational Generic Queue implementation (FIFO - First In, First Out).
/// </summary>
public class CustomQueue<T>
{
    private readonly LinkedList<T> _elements = new();

    public int Count => _elements.Count;
    public bool IsEmpty => _elements.Count == 0;

    /// <summary>
    /// Adds an item to the end of the queue.
    /// Time Complexity: O(1)
    /// </summary>
    public void Enqueue(T item)
    {
        _elements.AddLast(item);
    }

    /// <summary>
    /// Removes and returns the item at the front of the queue.
    /// Time Complexity: O(1)
    /// </summary>
    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Queue is empty.");

        T item = _elements.First!.Value;
        _elements.RemoveFirst();
        return item;
    }

    /// <summary>
    /// Returns the item at the front without removing it.
    /// Time Complexity: O(1)
    /// </summary>
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Queue is empty.");

        return _elements.First!.Value;
    }

    /// <summary>
    /// Returns snapshot of elements from front to back.
    /// </summary>
    public IReadOnlyList<T> ToList()
    {
        return new List<T>(_elements);
    }
}
