namespace ListenTogether.Pages;
public interface IDeviceManage
{
    Task ScreenOnAsync();
    Task ScreenOffAsync();
    Task<string> GetDeviceIdAsync();
}