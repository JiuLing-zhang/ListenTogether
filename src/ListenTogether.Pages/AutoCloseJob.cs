namespace ListenTogether.Pages;
public class AutoCloseJob
{
    private readonly IAppClose _appClose;
    private DateTime _closeTime;
    private System.Timers.Timer _time;
    public bool IsRunning => _time == null ? false : _time.Enabled;
    public AutoCloseJob(IAppClose appClose)
    {
        _appClose = appClose;
        _time = new System.Timers.Timer();
        _time.Interval = 1000;
        _time.Elapsed += _time_Elapsed;
    }

    private async void _time_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        if (DateTime.Now.Subtract(_closeTime).TotalSeconds >= 0)
        {
            await _appClose.CloseAsync();
        }
    }

    /// <summary>
    /// 启动定时关闭
    /// </summary>
    /// <param name="minute"></param>
    public void Start(int minute)
    {
        _closeTime = DateTime.Now.AddMinutes(minute);
        if (_time.Enabled)
        {
            return;
        }
        _time.Start();
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void Stop()
    {
        if (!_time.Enabled)
        {
            return;
        }
        _time.Stop();
    }

    /// <summary>
    /// 获取距离关闭的时间
    /// </summary>
    /// <returns></returns>
    public TimeSpan GetRemainingTime()
    {
        if (!_time.Enabled)
        {
            return TimeSpan.FromSeconds(0);
        }
        return _closeTime.Subtract(DateTime.Now);
    }

}