using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class MusicFileViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _fullName;

    [ObservableProperty]
    private Int64 _size;

    [ObservableProperty]
    private bool _isChecked;
}