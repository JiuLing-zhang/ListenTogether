namespace ListenTogether;
public static class Config
{
    public static bool Desktop
    {
        get
        {
#if WINDOWS || MACCATALYST
                return true;
#else
            return false;
#endif
        }
    }

    public static bool IsDarkTheme
    {
        get
        {
            if (App.Current.UserAppTheme == AppTheme.Dark)
            {
                return true;
            }
            return false;
        }
    }
}
