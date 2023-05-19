namespace ListenTogether.Pages;
public interface IMusicShare
{
    Task ShareLinkAsync(string url, string musicInfo);
}