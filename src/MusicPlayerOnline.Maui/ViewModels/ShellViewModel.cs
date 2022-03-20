using MusicPlayerOnline.Maui.Models;
using MusicPlayerOnline.Maui.Pages;
namespace MusicPlayerOnline.Maui.ViewModels;
internal class ShellViewModel
{
    public AppSection Playlist { get; set; }
    public AppSection MyFavorite { get; set; }
    public AppSection Playing { get; set; }
    public AppSection Settings { get; set; }

    public ShellViewModel()
    {
        Playlist = new AppSection() { Title = "播放列表", Icon = "playlist.png", IconDark = "playlist.png", TargetType = typeof(PlaylistPage) };
        MyFavorite = new AppSection() { Title = "我的歌单", Icon = "my_favorite.png", IconDark = "my_favorite.png", TargetType = typeof(MyFavoritePage) };
        Playing = new AppSection() { Title = "正在播放", Icon = "playing.png", IconDark = "playing.png", TargetType = typeof(PlayingPage) };
        Settings = new AppSection() { Title = "设置", Icon = "settings.png", IconDark = "settings.png", TargetType = typeof(SettingsPage) };
    }
}
