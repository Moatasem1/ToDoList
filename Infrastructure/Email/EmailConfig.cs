namespace Infrastructure.Email;

public class EmailConfig{
    public required string SenderName { get; set; }
    public required string SenderEmail { get; set; }
    public required string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public required string EmailPassword { get; set; }
}
