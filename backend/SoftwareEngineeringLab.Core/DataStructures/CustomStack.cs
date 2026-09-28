namespace SoftwareEngineeringLab.Core.DataStructures;

/// <summary>
/// Educational Generic Stack implementation (LIFO - Last In, First Out).
/// </summary>
public class CustomStack<T>
{
    private readonly List<T> _elements = new();

    public int Count => _elements.Count;
    public bool IsEmpty => _elements.Count == 0;

    /// <summary>
    /// Pushes an item onto the top of the stack.
    /// Time Complexity: O(1) amortized
    /// </summary>
    public void Push(T item)
    {
        _elements.Add(item);
    }

    /// <summary>
    /// Removes and returns the item at the top of the stack.
    /// Time Complexity: O(1)
    /// </summary>
    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Stack is empty.");

        int lastIndex = _elements.Count - 1;
        T item = _elements[lastIndex];
        _elements.RemoveAt(lastIndex);
        return item;
    }

    /// <summary>
    /// Returns the item at the top without removing it.
    /// Time Complexity: O(1)
    /// </summary>
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Stack is empty.");

        return _elements[^1];
    }

    /// <summary>
    /// Returns snapshot of elements from top to bottom.
    /// </summary>
    public IReadOnlyList<T> ToList()
    {
        var copy = new List<T>(_elements);
        copy.Reverse();
        return copy;
    }
}
