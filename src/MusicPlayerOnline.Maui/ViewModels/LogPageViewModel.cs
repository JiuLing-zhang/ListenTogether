using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels;

public class LogPageViewModel : ViewModelBase
{
    public ICommand UpdateLogsCommand => new Command(UpdateLogs);
    public ICommand ClearLogsCommand => new Command(ClearLogs);
    public LogPageViewModel()
    {
        Logs = new ObservableCollection<LogDetailViewModel>();
    }

    private ObservableCollection<LogDetailViewModel> _logs;
    public ObservableCollection<LogDetailViewModel> Logs
    {
        get => _logs;
        set
        {
            _logs = value;
            OnPropertyChanged();
        }
    }

    public async Task InitializeAsync()
    {
        if (Logs.Count > 0)
        {
            Logs.Clear();
        }

        var logs = Logger.GetAll();
        foreach (var log in logs)
        {
            Logs.Add(new LogDetailViewModel()
            {
                Time = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(log.CreateTime).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                LogType = log.LogType,
                Message = log.Message,
            });
        }
    }

    private async void UpdateLogs()
    {
        ToastService.Show("上传日志");
    }

    private async void ClearLogs()
    {
        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要清空日志吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        Logger.RemoveAllAsync();
        Logs.Clear();
    }
}