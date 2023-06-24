using JiuLing.CommonLibs.Log;
using ListenTogether.Api.Interfaces;
using ListenTogether.Api.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using ILogger = JiuLing.CommonLibs.Log.ILogger;

namespace ListenTogether.Api.Services;
public class MailService : IMailService
{
    private readonly ILogger _logger;
    private readonly MailSenderSettings _sender;
    public MailService(IOptions<MailSenderSettings> mailSenderSettings)
    {
        _logger = LogManager.GetLogger();
        _sender = mailSenderSettings.Value;
    }
    public async Task SendRegisterMailAsync(string email)
    {
        await SendMailAsync(email, "测试邮件", "test");
    }

    private async Task SendMailAsync(string email, string subject, string body)
    {
        try
        {
            MailMessage mailMessage = new MailMessage(_sender.Email, email);

            mailMessage.Subject = subject;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient(_sender.SmtpHost, _sender.SmtpPort);
            smtpClient.Credentials = new NetworkCredential(_sender.Email, _sender.Password);
            smtpClient.EnableSsl = true;

            smtpClient.SendAsync(mailMessage, null);
        }
        catch (Exception ex)
        {
            try
            {
                string message = "";
                message = $"{message}---------------{Environment.NewLine}";
                message = $"{message}邮件发送失败。{Environment.NewLine}";
                message = $"{message}{ex.Message}。{Environment.NewLine}";
                message = $"{message}{ex.StackTrace}。{Environment.NewLine}";
                var innerException = ex.InnerException;
                if (innerException != null)
                {
                    message = $"{message}↓↓↓↓↓InnerException↓↓↓↓↓{Environment.NewLine}";
                    message = $"{message}{innerException.Message}。{Environment.NewLine}";
                    message = $"{message}{innerException.StackTrace}。{Environment.NewLine}";
                }
                _logger.Write(message);
            }
            catch (Exception)
            {

            }
        }
    }
}