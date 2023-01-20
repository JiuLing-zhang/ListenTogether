using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Model.Enums;
using System.Collections.ObjectModel;
using System.Web;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(TagId), nameof(TagId))]
public partial class DiscoverViewModel : ViewModelBase
{
    private PlatformEnum _platform;
    private static Dictionary<PlatformEnum, DiscoverTag> _platformTag;

    [ObservableProperty]
    private string _tagId = null!;
    partial void OnTagIdChanged(string value)
    {
        if (value.IsEmpty())
        {
            return;
        }
        if (_platformTag.ContainsKey(_platform))
        {
            _platformTag[_platform].CurrentTag = value;
        }
    }

    [ObservableProperty]
    private ObservableCollection<MusicTagViewModel> _hotTags;

    [ObservableProperty]
    private ObservableCollection<SongMenuViewModel> _songMenus;

    private readonly IMusicNetworkService _musicNetworkService;

    public DiscoverViewModel(IMusicNetworkService musicNetworkService)
    {
        _musicNetworkService = musicNetworkService;

        HotTags = new ObservableCollection<MusicTagViewModel>();
        SongMenus = new ObservableCollection<SongMenuViewModel>();
        _platformTag = new Dictionary<PlatformEnum, DiscoverTag>();
    }
    public async Task InitializeAsync(PlatformEnum platform)
    {
        SongMenus.Clear();
        HotTags.Clear();

        if (!_platformTag.ContainsKey(platform))
        {
            var (hotTags, allTypes) = await _musicNetworkService.GetMusicTagsAsync(platform);
            _platformTag.Add(platform, new DiscoverTag("榜单", hotTags, allTypes));
        }
        _platform = platform;
        CreateHotTags(_platformTag[platform].HotTags);
        await GetTagDetailInnerAsync(_platformTag[platform].CurrentTag);
    }

    private void CreateHotTags(List<MusicTag> hotTags)
    {
        HotTags.Clear();
        HotTags.Add(new MusicTagViewModel()
        {
            Id = "榜单",
            Name = "榜单"
        });
        foreach (var hotTag in hotTags)
        {
            HotTags.Add(new MusicTagViewModel()
            {
                Id = hotTag.Id,
                Name = hotTag.Name,
            });
        }
        HotTags.Add(new MusicTagViewModel()
        {
            Id = "选择分类",
            Name = "选择分类"
        });
    }

    [RelayCommand]
    private async void GetTagDetailAsync(string id)
    {
        if (_platformTag.ContainsKey(_platform))
        {
            _platformTag[_platform].CurrentTag = id;
        }
        await GetTagDetailInnerAsync(id);
    }

    private async Task GetTagDetailInnerAsync(string tagId)
    {
        foreach (var tag in HotTags)
        {
            tag.BackgroundColor = tag.Id == tagId ? Color.FromArgb("#C98FFF") : Color.FromArgb("#00000000");
        }

        switch (tagId)
        {
            case "榜单":
                await GetSongMenusFromTop();
                break;
            case "选择分类":
                if (!_platformTag.ContainsKey(_platform))
                {
                    await ToastService.Show("平台信息未初始化完成");
                    return;
                }
                var json = _platformTag[_platform].AllTypes.ToJson();
                await Shell.Current.GoToAsync($"{nameof(ChooseTagPage)}?Json={json}");
                break;
            default:
                await GetSongMenusFromTagAsync(tagId);
                break;
        }
    }
    private Task GetSongMenusFromTop()
    {
        SongMenus.Clear();
        var songMenus = _musicNetworkService.GetSongMenusFromTop(_platform).Result;
        foreach (var songMenu in songMenus)
        {
            SongMenus.Add(new SongMenuViewModel()
            {
                SongMenuType = SongMenuEnum.Top,
                Id = songMenu.Id,
                Name = songMenu.Name,
                ImageUrl = songMenu.ImageUrl,
                LinkUrl = songMenu.LinkUrl
            });
        }
        return Task.CompletedTask;
    }
    private async Task GetSongMenusFromTagAsync(string id)
    {
        SongMenus.Clear();
        var songMenus = await _musicNetworkService.GetSongMenusFromTagAsync(_platform, id);
        foreach (var songMenu in songMenus)
        {
            SongMenus.Add(new SongMenuViewModel()
            {
                SongMenuType = SongMenuEnum.Tag,
                Id = songMenu.Id,
                Name = songMenu.Name,
                ImageUrl = songMenu.ImageUrl,
                LinkUrl = songMenu.LinkUrl
            }); ;
        }
    }

    [RelayCommand]
    private async void GotoSongMenuPageAsync(SongMenuViewModel songMenu)
    {
        string json = HttpUtility.UrlEncode(songMenu.ToJson());
        await Shell.Current.GoToAsync($"{nameof(SongMenuPage)}?Json={json}&PlatformString={_platform}");
    }
}