namespace ListenTogether.Api.Interfaces;
public interface IMailService
{
    Task SendRegisterMailAsync(string email);
}