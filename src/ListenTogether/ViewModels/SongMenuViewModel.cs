using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class SongMenuViewModel : ObservableObject
{
    [ObservableProperty]
    private string _id = null!;

    [ObservableProperty]
    private string _name = null!;

    [ObservableProperty]
    private string _linkUrl = null!;

    [ObservableProperty]
    private string _imageUrl = null!;
}