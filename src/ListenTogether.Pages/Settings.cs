using ListenTogether.Model;

namespace ListenTogether.Pages;
public class Settings
{
    public static EnvironmentSetting Environment { get; set; } = new EnvironmentSetting();

    public static PlatformEnum Platform { get; set;}
}

public enum PlatformEnum
{
    Phone = 0,
    Desktop = 1,
    Web = 2
}