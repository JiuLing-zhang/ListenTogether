using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class MusicFileViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = null!;

    [ObservableProperty]
    private string _fullName = null!;

    [ObservableProperty]
    private Int64 _size;

    [ObservableProperty]
    private bool _isChecked;
}