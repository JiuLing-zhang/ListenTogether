using System.Collections.ObjectModel;

namespace ListenTogether.Controls;

public partial class MusicResultView
{
    public static readonly BindableProperty MusicsProperty =
        BindableProperty.Create(
            nameof(Musics),
            typeof(ObservableCollection<MusicResultGroupViewModel>),
            typeof(MusicResultView),
            new ObservableCollection<MusicResultGroupViewModel>());
    public ObservableCollection<MusicResultGroupViewModel> Musics
    {
        get { return (ObservableCollection<MusicResultGroupViewModel>)GetValue(MusicsProperty); }
        set { SetValue(MusicsProperty, value); }
    }
    public MusicResultView()
    {
        InitializeComponent();
    }
}