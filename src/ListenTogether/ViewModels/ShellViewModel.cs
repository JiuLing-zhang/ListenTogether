using CommunityToolkit.Mvvm.ComponentModel;
using ListenTogether.Models;
namespace ListenTogether.ViewModels;
internal partial class ShellViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _icon;

    public AppSection SearchResult { get; set; }
    public AppSection Playlist { get; set; }
    public AppSection MyFavorite { get; set; }
    public AppSection Settings { get; set; }

    public ShellViewModel()
    {
        SearchResult = new AppSection() { Title = "搜索", Icon = "search.png", IconDark = "search_dark.png", TargetType = typeof(SearchResultPage) };
        Playlist = new AppSection() { Title = "播放列表", Icon = "playlist.png", IconDark = "playlist_dark.png", TargetType = typeof(PlaylistPage) };
        MyFavorite = new AppSection() { Title = "我的歌单", Icon = "heart.png", IconDark = "heart_dark.png", TargetType = typeof(MyFavoritePage) };
        Settings = new AppSection() { Title = "我的", Icon = "my.png", IconDark = "my_dark.png", TargetType = typeof(SettingsPage) };
    }
}
