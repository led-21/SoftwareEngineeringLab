namespace SoftwareEngineeringLab.Core.DesignPatterns.Behavioral;

public interface ICommand
{
    string Description { get; }
    void Execute();
    void Undo();
}

public class TextEditorDocument
{
    public string Content { get; set; } = string.Empty;
}

public class AppendTextCommand : ICommand
{
    private readonly TextEditorDocument _document;
    private readonly string _textToAppend;

    public string Description => $"Append \"{_textToAppend}\"";

    public AppendTextCommand(TextEditorDocument document, string textToAppend)
    {
        _document = document;
        _textToAppend = textToAppend;
    }

    public void Execute()
    {
        _document.Content += _textToAppend;
    }

    public void Undo()
    {
        if (_document.Content.EndsWith(_textToAppend))
        {
            _document.Content = _document.Content[..^_textToAppend.Length];
        }
    }
}

/// <summary>
/// Command Pattern: Encapsulates a request as an object, enabling parameterization, queuing, and undo/redo operations.
/// </summary>
public class CommandHistoryInvoker
{
    private readonly Stack<ICommand> _history = new();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public bool UndoLast()
    {
        if (_history.Count == 0)
            return false;

        var command = _history.Pop();
        command.Undo();
        return true;
    }
}
