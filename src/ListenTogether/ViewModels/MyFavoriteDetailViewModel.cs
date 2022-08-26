using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public partial class MyFavoriteDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _seq;

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
}