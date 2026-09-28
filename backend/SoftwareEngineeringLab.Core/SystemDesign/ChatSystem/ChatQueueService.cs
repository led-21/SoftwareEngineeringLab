using System.Collections.Concurrent;

namespace SoftwareEngineeringLab.Core.SystemDesign.ChatSystem;

public record ChatMessage(
    string Id,
    string SenderId,
    string ReceiverId,
    string Content,
    DateTime TimestampUtc
);

/// <summary>
/// In-memory asynchronous message broker for 1:1 chat with long-polling waiting support.
/// </summary>
public class ChatQueueService
{
    private readonly ConcurrentDictionary<string, ConcurrentQueue<ChatMessage>> _userInboxes = new();
    private readonly ConcurrentDictionary<string, TaskCompletionSource<bool>> _waitingReceivers = new();

    public void SendMessage(ChatMessage message)
    {
        var inbox = _userInboxes.GetOrAdd(message.ReceiverId, _ => new ConcurrentQueue<ChatMessage>());
        inbox.Enqueue(message);

        // Notify awaiting subscriber if one exists
        if (_waitingReceivers.TryRemove(message.ReceiverId, out var tcs))
        {
            tcs.TrySetResult(true);
        }
    }

    public async Task<ChatMessage?> ReceiveMessageAsync(string userId, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        var inbox = _userInboxes.GetOrAdd(userId, _ => new ConcurrentQueue<ChatMessage>());

        if (inbox.TryDequeue(out var immediateMessage))
        {
            return immediateMessage;
        }

        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        _waitingReceivers[userId] = tcs;

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);

        try
        {
            using (cts.Token.Register(() => tcs.TrySetCanceled()))
            {
                await tcs.Task;
            }
            return inbox.TryDequeue(out var arrivedMessage) ? arrivedMessage : null;
        }
        catch (OperationCanceledException)
        {
            _waitingReceivers.TryRemove(userId, out _);
            return null;
        }
    }

    public int GetPendingCount(string userId)
    {
        return _userInboxes.TryGetValue(userId, out var inbox) ? inbox.Count : 0;
    }
}
