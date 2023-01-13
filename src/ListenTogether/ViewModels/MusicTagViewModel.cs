using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class MusicTagViewModel : ObservableObject
{
    [ObservableProperty]
    private string _id = null!;

    [ObservableProperty]
    private string _name = null!;

    [ObservableProperty]
    private Color _backgroundColor = null!;
}