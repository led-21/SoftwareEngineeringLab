using SoftwareEngineeringLab.Core.Algorithms.LinkedLists;
using Xunit;

namespace SoftwareEngineeringLab.Tests.Algorithms;

public class LinkedListProblemsTests
{
    private readonly LinkedListProblems _sut = new();

    [Fact]
    public void ReverseList_ReversesSequence()
    {
        var head = ListNode.FromArray(new[] { 1, 2, 3, 4 });
        var reversed = _sut.ReverseList(head);
        Assert.Equal(new[] { 4, 3, 2, 1 }, reversed?.ToList());
    }

    [Fact]
    public void MergeTwoLists_CombinesSortedListsInOrder()
    {
        var list1 = ListNode.FromArray(new[] { 1, 2, 4 });
        var list2 = ListNode.FromArray(new[] { 1, 3, 4 });

        var merged = _sut.MergeTwoLists(list1, list2);
        Assert.Equal(new[] { 1, 1, 2, 3, 4, 4 }, merged?.ToList());
    }

    [Fact]
    public void HasCycle_DetectsCyclicAndAcyclicLists()
    {
        var node1 = new ListNode(1);
        var node2 = new ListNode(2);
        var node3 = new ListNode(3);
        node1.Next = node2;
        node2.Next = node3;

        Assert.False(_sut.HasCycle(node1));

        node3.Next = node2; // Form cycle
        Assert.True(_sut.HasCycle(node1));
    }

    [Fact]
    public void RemoveNthFromEnd_RemovesTargetNode()
    {
        var head = ListNode.FromArray(new[] { 1, 2, 3, 4, 5 });
        var updated = _sut.RemoveNthFromEnd(head, 2); // Removes 4
        Assert.Equal(new[] { 1, 2, 3, 5 }, updated?.ToList());
    }
}
