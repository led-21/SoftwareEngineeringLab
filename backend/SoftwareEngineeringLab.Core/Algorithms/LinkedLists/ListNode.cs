namespace SoftwareEngineeringLab.Core.Algorithms.LinkedLists;

public class ListNode
{
    public int Val { get; set; }
    public ListNode? Next { get; set; }

    public ListNode(int val = 0, ListNode? next = null)
    {
        Val = val;
        Next = next;
    }

    public static ListNode? FromArray(int[] values)
    {
        if (values == null || values.Length == 0)
            return null;

        var head = new ListNode(values[0]);
        var current = head;
        for (int i = 1; i < values.Length; i++)
        {
            current.Next = new ListNode(values[i]);
            current = current.Next;
        }
        return head;
    }

    public List<int> ToList()
    {
        var result = new List<int>();
        var current = this;
        var visited = new HashSet<ListNode>();

        while (current != null && visited.Add(current))
        {
            result.Add(current.Val);
            current = current.Next;
        }

        return result;
    }
}
