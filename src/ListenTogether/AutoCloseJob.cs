namespace ListenTogether;

/// <summary>
/// 定时关闭的任务
/// </summary>
internal class AutoCloseJob
{
    private static DateTime _closeTime;
    private static IDispatcherTimer _time;
    public static bool IsRunning => _time == null ? false : _time.IsRunning;
    private AutoCloseJob()
    {

    }

    public static void Initialize()
    {
        _time = App.Current.Dispatcher.CreateTimer();
        _time.Interval = TimeSpan.FromMilliseconds(1000);
        _time.Tick += _time_Tick;
    }

    /// <summary>
    /// 启动定时关闭
    /// </summary>
    /// <param name="minute"></param>
    public static void Start(int minute)
    {
        _closeTime = DateTime.Now.AddMinutes(minute);
        if (_time.IsRunning)
        {
            return;
        }
        _time.Start();
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public static void Stop()
    {
        if (!_time.IsRunning)
        {
            return;
        }
        _time.Stop();
    }

    /// <summary>
    /// 获取距离关闭的时间
    /// </summary>
    /// <returns></returns>
    public static TimeSpan GetRemainingTime()
    {
        return _closeTime.Subtract(DateTime.Now);
    }

    private static void _time_Tick(object sender, EventArgs e)
    {
        if (DateTime.Now.Subtract(_closeTime).TotalSeconds >= 0)
        {
            Application.Current.Quit();
        }
    }
}