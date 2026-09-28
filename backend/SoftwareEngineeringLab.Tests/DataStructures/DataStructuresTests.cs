using SoftwareEngineeringLab.Core.DataStructures;
using Xunit;

namespace SoftwareEngineeringLab.Tests.DataStructures;

public class DataStructuresTests
{
    [Fact]
    public void CustomStack_MaintainsLifoOrder()
    {
        var stack = new CustomStack<int>();
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);

        Assert.Equal(3, stack.Count);
        Assert.Equal(30, stack.Peek());
        Assert.Equal(30, stack.Pop());
        Assert.Equal(20, stack.Pop());
        Assert.Equal(10, stack.Pop());
        Assert.True(stack.IsEmpty);
    }

    [Fact]
    public void CustomQueue_MaintainsFifoOrder()
    {
        var queue = new CustomQueue<string>();
        queue.Enqueue("first");
        queue.Enqueue("second");
        queue.Enqueue("third");

        Assert.Equal(3, queue.Count);
        Assert.Equal("first", queue.Peek());
        Assert.Equal("first", queue.Dequeue());
        Assert.Equal("second", queue.Dequeue());
        Assert.Equal("third", queue.Dequeue());
        Assert.True(queue.IsEmpty);
    }
}
