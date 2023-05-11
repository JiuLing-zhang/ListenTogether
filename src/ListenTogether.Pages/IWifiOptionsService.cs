namespace ListenTogether.Pages;
public interface IWifiOptionsService
{
    Task<bool> HasWifiOrCanPlayAsync();
}