namespace MusicPlayerOnline.Maui.ViewModels;

public class LogDetailViewModel : ViewModelBase
{
    private string _time;
    public string Time
    {
        get => _time;
        set
        {
            _time = value;
            OnPropertyChanged();
        }
    }

    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }
}