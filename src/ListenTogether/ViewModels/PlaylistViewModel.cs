using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public partial class PlaylistViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _platformName;

    [ObservableProperty]
    private string _musicId;

    [ObservableProperty]
    private string _musicName;

    [ObservableProperty]
    private string _musicArtist;

    [ObservableProperty]
    private string _musicAlbum;

    public DateTime EditTime { get; set; }
}