namespace ListenTogether.ViewModels;

public class UserInfoViewModel : ViewModelBase
{

    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    private string _nickname;
    public string Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
            OnPropertyChanged();
        }
    }

    private string _avatar;
    public string Avatar
    {
        get => _avatar;
        set
        {
            _avatar = value;
            OnPropertyChanged();
        }
    }
}