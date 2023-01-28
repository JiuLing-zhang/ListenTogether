using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;

public partial class MyFavoriteDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _seq;

    [ObservableProperty]
    private MusicResultShowViewModel _music;
}