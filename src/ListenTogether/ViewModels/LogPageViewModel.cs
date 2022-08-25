using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public partial class LogPageViewModel : ObservableObject
{
    private IApiLogService _apiLogService;
    public LogPageViewModel(IApiLogService apiLogService)
    {
        Logs = new ObservableCollection<LogDetailViewModel>();
        _apiLogService = apiLogService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;

            IsLoginSuccess = GlobalConfig.CurrentUser != null;

            if (Logs.Count > 0)
            {
                Logs.Clear();
            }

            _updateLogs = new List<Log>();
            var logs = Logger.GetAll();
            foreach (var log in logs)
            {
                //页面展示
                Logs.Add(new LogDetailViewModel()
                {
                    Time = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(log.CreateTime).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    Message = log.Message,
                });

                //后台保存，用于上传
                _updateLogs.Add(new Log()
                {
                    Timestamp = log.CreateTime,
                    LogType = log.LogType,
                    Message = log.Message
                });
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("日志加载失败");
            Logger.Error("日志页面初始化失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [ObservableProperty]
    private bool _isLoginSuccess;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    public bool IsNotBusy => !_isBusy;

    [ObservableProperty]
    private ObservableCollection<LogDetailViewModel> _logs;

    private List<Log> _updateLogs;

    [RelayCommand]
    private async void UpdateLogs()
    {
        if (GlobalConfig.CurrentUser == null)
        {
            await ToastService.Show("上传失败，用户未登录");
            return;
        }
        if (_updateLogs.Count == 0)
        {
            await ToastService.Show("没有要上传的日志");
            return;
        }

        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要上传日志到服务器吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var result = await _apiLogService.WriteListAsync(_updateLogs);
            if (result == false)
            {
                await ToastService.Show("日志上传失败");
            }
            else
            {
                await ToastService.Show("日志上传成功");
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("上传失败，网络出小差了");
            Logger.Error("日志上传失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async void ClearLogs()
    {
        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要清空日志吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        Logger.RemoveAllAsync();
        await InitializeAsync();
    }
}