namespace ListenTogether.Api.Models;
public class MailSenderSettings
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string SmtpHost { get; set; } = null!;
    public int SmtpPort { get; set; }
}