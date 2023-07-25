using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Service.Interface;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public partial class LogPageViewModel : ViewModelBase
{
    private readonly ILogManage _logManage;
    public LogPageViewModel(ILogManage logManage)
    {
        _logManage = logManage;
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
            var logs = await _logManage.GetAllAsync();
            foreach (var log in logs)
            {
                //页面展示
                Logs.Add(new LogDetailViewModel()
                {
                    Time = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(log.Timestamp).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    Message = log.Message,
                });

                //后台保存，用于上传
                _updateLogs.Add(log);
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("日志加载失败");
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
        await _logManage.RemoveAllAsync();
        await InitializeAsync();
    }
}