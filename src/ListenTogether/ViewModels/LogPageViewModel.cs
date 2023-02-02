using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public partial class LogPageViewModel : ViewModelBase
{
    public LogPageViewModel()
    {
        Logs = new ObservableCollection<LogDetailViewModel>();
    }

    public async Task InitializeAsync()
    {
        try
        {
            Loading("日志加载中");
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
            LoadComplete();
        }
    }

    [ObservableProperty]
    private ObservableCollection<LogDetailViewModel> _logs = null!;

    private List<Log> _updateLogs = null!;

    [RelayCommand]
    private async void ClearLogsAsync()
    {
        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要清空日志吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        Logger.RemoveAll();
        await InitializeAsync();
    }
}