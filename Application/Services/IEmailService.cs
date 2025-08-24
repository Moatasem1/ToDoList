namespace Application.Services;

public interface IEmailService
{
    public static class EmailTemplatePaths
    {
        public static readonly string AccountCreated = "AccountCreated";
    }
    Task SendAccountCreatedEmail(string userEmail, string userName, string password);
}
