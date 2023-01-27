using System.Collections.ObjectModel;

namespace ListenTogether.Controls;

public partial class MusicResultView
{
    public bool IsMultiPlatform => Musics.Count > 1;
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

    public static readonly BindableProperty PlayCommandProperty =
        BindableProperty.Create(
            nameof(PlayCommand),
            typeof(ICommand),
            typeof(MusicResultView));

    public static readonly BindableProperty PlayCommandParameterProperty =
        BindableProperty.Create(
            nameof(PlayCommandParameter),
            typeof(MusicResultShowViewModel),
            typeof(MusicResultView));

    public ICommand PlayCommand
    {
        get { return (ICommand)GetValue(PlayCommandProperty); }
        set { SetValue(PlayCommandProperty, value); }
    }

    public MusicResultShowViewModel PlayCommandParameter
    {
        get { return (MusicResultShowViewModel)GetValue(PlayCommandParameterProperty); }
        set { SetValue(PlayCommandParameterProperty, value); }
    }

    public static readonly BindableProperty AddToFavoriteCommandProperty =
      BindableProperty.Create(
          nameof(AddToFavoriteCommand),
          typeof(ICommand),
          typeof(MusicResultView));

    public static readonly BindableProperty AddToFavoriteCommandParameterProperty =
        BindableProperty.Create(
            nameof(AddToFavoriteCommandParameter),
            typeof(MusicResultShowViewModel),
            typeof(MusicResultView));

    public ICommand AddToFavoriteCommand
    {
        get { return (ICommand)GetValue(AddToFavoriteCommandProperty); }
        set { SetValue(AddToFavoriteCommandProperty, value); }
    }

    public MusicResultShowViewModel AddToFavoriteCommandParameter
    {
        get { return (MusicResultShowViewModel)GetValue(AddToFavoriteCommandParameterProperty); }
        set { SetValue(AddToFavoriteCommandParameterProperty, value); }
    }
    public MusicResultView()
    {
        InitializeComponent();
    }
}