namespace SoftwareEngineeringLab.Core.Solid.DependencyInversion;

// ==================== DEPENDENCY INVERSION PRINCIPLE (DIP) ====================
// "High-level modules should not depend on low-level modules. Both should depend on abstractions."

public static class DependencyInversionStudy
{
    // ❌ VIOLATION: NotificationService directly instantiates SmtpSender (tight coupling).
    // Testing is hard (requires real SMTP server) and switching to SMS/Push requires editing this class.
    public class SmtpSenderConcrete
    {
        public string SendEmail(string message) => $"[SMTP] {message}";
    }

    public class NotificationServiceBad
    {
        private readonly SmtpSenderConcrete _sender = new();

        public string SendAlert(string message)
        {
            return _sender.SendEmail(message);
        }
    }

    //  REFACTORED: NotificationService depends on the IMessageSender abstraction.

    public interface IMessageSender
    {
        string Send(string recipient, string message);
    }

    public class EmailMessageSender : IMessageSender
    {
        public string Send(string recipient, string message) => $"[Email to {recipient}]: {message}";
    }

    public class SmsMessageSender : IMessageSender
    {
        public string Send(string recipient, string message) => $"[SMS to {recipient}]: {message}";
    }

    public class NotificationService
    {
        private readonly IMessageSender _messageSender;

        public NotificationService(IMessageSender messageSender)
        {
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        public string Notify(string recipient, string message)
        {
            return _messageSender.Send(recipient, message);
        }
    }
}
