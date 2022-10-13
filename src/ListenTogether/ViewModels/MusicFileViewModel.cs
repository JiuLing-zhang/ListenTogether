using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class MusicFileViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _remark = null!;

    [ObservableProperty]
    private string _fileName = null!;

    [ObservableProperty]
    private Int64 _size;

    [ObservableProperty]
    private bool _isChecked;
}