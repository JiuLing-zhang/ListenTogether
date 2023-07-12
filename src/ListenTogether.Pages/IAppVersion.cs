namespace ListenTogether.Pages;
public interface IAppVersion
{
    Task<Version> GetCurrentVersionAsync();
    Task<string> GetCurrentVersionStringAsync();
}