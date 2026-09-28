namespace SoftwareEngineeringLab.Core.Algorithms.LinkedLists;

/// <summary>
/// Classic linked list interview problems.
/// </summary>
public class LinkedListProblems
{
    /// <summary>
    /// Reverse Linked List: Reverses a singly-linked list in place.
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public ListNode? ReverseList(ListNode? head)
    {
        ListNode? prev = null;
        ListNode? current = head;

        while (current != null)
        {
            ListNode? next = current.Next;
            current.Next = prev;
            prev = current;
            current = next;
        }
        return prev;
    }

    /// <summary>
    /// Merge Two Sorted Lists: Splicing together the nodes of the first two lists.
    /// Time Complexity: O(n + m)
    /// Space Complexity: O(1)
    /// </summary>
    public ListNode? MergeTwoLists(ListNode? list1, ListNode? list2)
    {
        var dummy = new ListNode(0);
        var current = dummy;

        while (list1 != null && list2 != null)
        {
            if (list1.Val <= list2.Val)
            {
                current.Next = list1;
                list1 = list1.Next;
            }
            else
            {
                current.Next = list2;
                list2 = list2.Next;
            }
            current = current.Next;
        }

        current.Next = list1 ?? list2;
        return dummy.Next;
    }

    /// <summary>
    /// Linked List Cycle Detection (Floyd's Tortoise and Hare).
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public bool HasCycle(ListNode? head)
    {
        if (head?.Next == null)
            return false;

        ListNode? slow = head;
        ListNode? fast = head.Next;

        while (slow != fast)
        {
            if (fast?.Next == null)
                return false;

            slow = slow?.Next;
            fast = fast.Next.Next;
        }
        return true;
    }

    /// <summary>
    /// Remove Nth Node From End of List (Two-pointer technique).
    /// Time Complexity: O(n)
    /// Space Complexity: O(1)
    /// </summary>
    public ListNode? RemoveNthFromEnd(ListNode? head, int n)
    {
        if (head == null || n <= 0)
            return head;

        var dummy = new ListNode(0, head);
        ListNode? fast = dummy;
        ListNode? slow = dummy;

        for (int i = 0; i <= n; i++)
        {
            if (fast == null)
                return head;
            fast = fast.Next;
        }

        while (fast != null)
        {
            fast = fast.Next;
            slow = slow?.Next;
        }

        if (slow?.Next != null)
            slow.Next = slow.Next.Next;

        return dummy.Next;
    }
}
