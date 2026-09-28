namespace SoftwareEngineeringLab.Core.DesignPatterns.Structural;

// Modern target interface expected by the application
public interface IModernJsonLogger
{
    string LogJson(string message, string level);
}

// Incompatible legacy service with XML output
public class LegacyXmlLogger
{
    public string LogXml(string xmlPayload) =>
        $"<LogEntry><Timestamp>{DateTime.UtcNow:O}</Timestamp><Payload>{xmlPayload}</Payload></LegacyXmlLogger>";
}

/// <summary>
/// Adapter Pattern: Converts the interface of a class into another interface clients expect.
/// </summary>
public class XmlToJsonLoggerAdapter : IModernJsonLogger
{
    private readonly LegacyXmlLogger _legacyLogger;

    public XmlToJsonLoggerAdapter(LegacyXmlLogger legacyLogger)
    {
        _legacyLogger = legacyLogger ?? throw new ArgumentNullException(nameof(legacyLogger));
    }

    public string LogJson(string message, string level)
    {
        // Adapts modern input into legacy XML format
        string xmlPayload = $"<Level>{level}</Level><Message>{message}</Message>";
        return _legacyLogger.LogXml(xmlPayload);
    }
}
