using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public partial class MyFavoriteDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _seq;

    [ObservableProperty]
    private string _platformName = null!;

    [ObservableProperty]
    private string _musicId = null!;

    [ObservableProperty]
    private string _musicName = null!;

    [ObservableProperty]
    private string _musicArtist = null!;

    [ObservableProperty]
    private string _musicAlbum = null!;
}