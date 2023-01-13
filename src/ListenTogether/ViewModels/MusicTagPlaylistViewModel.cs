using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class MusicTagPlaylistViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = null!;

    [ObservableProperty]
    private string _linkUrl = null!;

    [ObservableProperty]
    private string _imageUrl = null!;
}