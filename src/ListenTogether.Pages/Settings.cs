using ListenTogether.Model;

namespace ListenTogether.Pages;
public class Settings
{
    public static EnvironmentSetting Environment { get; set; } = new EnvironmentSetting();

    public static OSTypeEnum OSType { get; set; }
}

public enum OSTypeEnum
{
    Phone = 0,
    Desktop = 1,
    Web = 2
}