namespace ListenTogether.Pages;
public interface IAppVersion
{
    Version GetCurrentVersion();
    string GetCurrentVersionString();
}