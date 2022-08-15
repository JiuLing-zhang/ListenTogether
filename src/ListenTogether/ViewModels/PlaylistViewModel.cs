namespace ListenTogether.ViewModels;

public class PlaylistViewModel : ViewModelBase
{
    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }

    private string _platformName;
    public string PlatformName
    {
        get => _platformName;
        set
        {
            _platformName = value;
            OnPropertyChanged();
        }
    }

    private string _musicId;
    public string MusicId
    {
        get => _musicId;
        set
        {
            _musicId = value;
            OnPropertyChanged();
        }
    }

    private string _musicName;
    public string MusicName
    {
        get => _musicName;
        set
        {
            _musicName = value;
            OnPropertyChanged();
        }
    }

    private string _musicArtist;
    public string MusicArtist
    {
        get => _musicArtist;
        set
        {
            _musicArtist = value;
            OnPropertyChanged();
        }
    }

    private string _musicAlbum;
    public string MusicAlbum
    {
        get => _musicAlbum;
        set
        {
            _musicAlbum = value;
            OnPropertyChanged();
        }
    }
    public DateTime EditTime { get; set; }
}