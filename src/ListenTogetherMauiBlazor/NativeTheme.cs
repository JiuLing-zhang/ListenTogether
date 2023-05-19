using CommunityToolkit.Maui.Core;
using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
internal class NativeTheme : INativeTheme
{
    public void SetTheme(bool isDark)
    {
        if (isDark)
        {
            App.Current.UserAppTheme = AppTheme.Dark;
#if ANDROID
            CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("#333333"));
            CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.DarkContent);
#endif
        }
        else
        {
            App.Current.UserAppTheme = AppTheme.Light;
#if ANDROID
            CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("#FFFFFF"));
            CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);
#endif
        }
    }
}