namespace SoftwareEngineeringLab.Core.Solid.SingleResponsibility;

// ==================== SINGLE RESPONSIBILITY PRINCIPLE (SRP) ====================
// "A class should have one, and only one, reason to change."

public static class SingleResponsibilityStudy
{
    public record User(string Username, string Email);

    // ❌ VIOLATION: UserManager handles both user persistence/business logic AND email notifications.
    // Reasons to change: User entity rules change, OR notification provider/templates change.
    public class UserManagerBad
    {
        public string CreateUser(string username, string email)
        {
            // Business logic
            var status = $"User {username} persisted to database.";

            // Email dispatch (violates SRP)
            SendEmail(email, "Welcome to the platform!");
            return status;
        }

        private void SendEmail(string email, string message)
        {
            // Direct coupling to emailing mechanism
        }
    }

    //  REFACTORED: Responsibilities segregated into focused classes.

    public interface IUserRepository
    {
        void Save(User user);
    }

    public interface IEmailNotificationService
    {
        void SendWelcomeEmail(string email);
    }

    public class UserService
    {
        private readonly IUserRepository _repository;
        private readonly IEmailNotificationService _emailService;

        public UserService(IUserRepository repository, IEmailNotificationService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public void RegisterUser(string username, string email)
        {
            var user = new User(username, email);
            _repository.Save(user);
            _emailService.SendWelcomeEmail(user.Email);
        }
    }
}
