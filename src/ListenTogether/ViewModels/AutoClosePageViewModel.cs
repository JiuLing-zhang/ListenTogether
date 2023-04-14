using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class AutoClosePageViewModel : ViewModelBase
{
    private static IDispatcherTimer _time;

    /// <summary>
    /// 是否启用自动关闭
    /// </summary>
    [ObservableProperty]
    private bool _isAutoClose;
    partial void OnIsAutoCloseChanged(bool value)
    {
        if (value)
        {
            if (AutoCloseMinute == 0 && AutoCloseCustomMinute == 0)
            {
                IsDefaultChecked = true;
            }
            AutoCloseJob.Start(GetUserTime());
        }
        else
        {
            IsCustomChecked = true;
            AutoCloseJob.Stop();
        }
    }

    /// <summary>
    /// 自动关闭的剩余时间
    /// </summary>
    [ObservableProperty]
    private string _remainingTimeString;

    //TODO 双向绑定有问题，临时解决方案
    [ObservableProperty]
    private bool _isDefaultChecked;
    [ObservableProperty]
    private bool _isCustomChecked;

    /// <summary>
    /// 自动关闭时间
    /// </summary>
    [ObservableProperty]
    private int _autoCloseMinute;
    partial void OnAutoCloseMinuteChanged(int value)
    {
        if (value == 0)
        {
            IsCustomChecked = true;
            return;
        }
        if (AutoCloseMinute != 0)
        {
            AutoCloseCustomMinute = 0;
        }

        if (!IsAutoClose)
        {
            IsAutoClose = true;
        }
        AutoCloseJob.Start(GetUserTime());
    }

    /// <summary>
    /// 自动关闭时间(自定义)
    /// </summary>
    [ObservableProperty]
    private int _autoCloseCustomMinute;
    partial void OnAutoCloseCustomMinuteChanged(int value)
    {
        if (value == 0)
        {
            return;
        }
        if (AutoCloseMinute != 0)
        {
            AutoCloseMinute = 0;
        }
        if (!IsAutoClose)
        {
            IsAutoClose = true;
        }
        AutoCloseJob.Start(GetUserTime());
    }

    public AutoClosePageViewModel()
    {
        _time = App.Current.Dispatcher.CreateTimer();
        _time.Interval = TimeSpan.FromMilliseconds(1000);
        _time.Tick += _time_Tick;
    }
    public Task Initialize()
    {
        _time.Start();
        IsAutoClose = AutoCloseJob.IsRunning;
        return Task.CompletedTask;
    }

    public Task Disappearing()
    {
        _time.Stop();
        return Task.CompletedTask;
    }

    private int GetUserTime()
    {
        int minute;
        if (AutoCloseMinute == 0)
        {
            minute = AutoCloseCustomMinute;
        }
        else
        {
            minute = AutoCloseMinute;
        }
        return minute;
    }
    private void _time_Tick(object sender, EventArgs e)
    {
        if (!AutoCloseJob.IsRunning)
        {
            if (RemainingTimeString.IsNotEmpty())
            {
                RemainingTimeString = "";
            }
            return;
        }

        var ts = AutoCloseJob.GetRemainingTime();
        RemainingTimeString = $"{(ts.Hours * 60 + ts.Minutes):D2}:{ts.Seconds:D2}";
    }
}