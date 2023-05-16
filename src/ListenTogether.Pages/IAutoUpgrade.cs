namespace ListenTogether.Pages;
public interface IAutoUpgrade
{
    Task DoAsync(bool isBackgroundCheck);
}