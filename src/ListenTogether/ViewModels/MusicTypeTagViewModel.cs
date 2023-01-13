using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;
public partial class MusicTypeTagViewModel : ObservableObject
{
    [ObservableProperty]
    private string _typeName = null!;

    [ObservableProperty]
    private ObservableCollection<MusicTagViewModel> _tags;
}