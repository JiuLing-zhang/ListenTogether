using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Model.Enums;
using System.Collections.ObjectModel;
using System.Web;

namespace ListenTogether.ViewModels;
public partial class DiscoverPageViewModel : ViewModelBase
{
    /// <summary>
    /// 当前标签歌单的页码
    /// </summary>
    private int _currentPage = 1;
    /// <summary>
    /// 当前标签的Id
    /// </summary>
    private string _currentTagId = "";

    [ObservableProperty]
    private ObservableCollection<DiscoverTabViewModel> _discoverTabs;

    [ObservableProperty]
    private ObservableCollection<DiscoverTagViewModel> _discoverTags;

    [ObservableProperty]
    private ObservableCollection<SongMenuViewModel> _songMenus;

    private static readonly object LockPlatformMusicTags = new object();
    private static readonly Dictionary<PlatformEnum, PlatformMusicTag> PlatformMusicTags = new Dictionary<PlatformEnum, PlatformMusicTag>();

    private readonly MusicNetPlatform _musicNetworkService;

    private PlatformEnum Platform => (PlatformEnum)DiscoverTabs.First(x => x.IsSelected).Id;

    private readonly SearchPage _searchPage;
    public DiscoverPageViewModel(MusicNetPlatform musicNetworkService, SearchPage searchPage)
    {
        _searchPage = searchPage;
        _musicNetworkService = musicNetworkService;
        DiscoverTabs = new ObservableCollection<DiscoverTabViewModel>
        {
            new()
            {
                Id=(int)PlatformEnum.MiGu,
                Name=PlatformEnum.MiGu.GetDescription(),
                IsSelected=true
            },
            new()
            {
                Id=(int)PlatformEnum.KuWo,
                Name=PlatformEnum.KuWo.GetDescription(),
                IsSelected=false
            },
            new()
            {
                Id=(int)PlatformEnum.NetEase,
                Name=PlatformEnum.NetEase.GetDescription(),
                IsSelected=false
            }
        };
        DiscoverTags = new ObservableCollection<DiscoverTagViewModel>();
        SongMenus = new ObservableCollection<SongMenuViewModel>();
        InitPlatformMusicTags();
    }

    private void InitPlatformMusicTags()
    {
        var tasks = new Task[DiscoverTabs.Count];
        for (int i = 0; i < DiscoverTabs.Count; i++)
        {
            var index = i;
            var platform = (PlatformEnum)DiscoverTabs[index].Id;
            tasks[index] = Task.Run(async () =>
            {
                try
                {
                    var platformMusicTag = await _musicNetworkService.GetMusicTagsAsync(platform);
                    lock (LockPlatformMusicTags)
                    {
                        PlatformMusicTags.Add(platform, platformMusicTag);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"发现页加载失败：{platform.GetDescription()}", ex);
                }
            });
        }
        Task.WaitAll(tasks);
    }
    public async Task InitializeAsync()
    {
        await SelectTab((int)PlatformEnum.MiGu);
    }

    [RelayCommand]
    private async void TabChangedAsync(int id)
    {
        if (DiscoverTabs.First(x => x.IsSelected).Id == id)
        {
            return;
        }
        await SelectTab(id);
    }

    [RelayCommand]
    private async void TagChangedAsync(string id)
    {
        var selectedTag = DiscoverTags.FirstOrDefault(x => x.IsSelected);
        if (selectedTag != null && selectedTag.Id == id)
        {
            return;
        }
        await SelectMusicTag(id);
    }
    /// <summary>
    /// 选择标签
    /// </summary>
    /// <param name="id"></param>
    private async Task SelectTab(int id)
    {
        foreach (var tab in DiscoverTabs)
        {
            if (tab.Id == id)
            {
                tab.IsSelected = true;
            }
            else
            {
                tab.IsSelected = false;
            }
        }
        InitMusicTags();
        await SelectMusicTag("榜单");
    }

    private void InitMusicTags()
    {
        if (!PlatformMusicTags.TryGetValue(Platform, out var platformMusicTag))
        {
            return;
        }

        DiscoverTags.Clear();
        DiscoverTags.Add(new DiscoverTagViewModel()
        {
            Id = "榜单",
            Name = "榜单",
            IsSelected = false
        });
        foreach (var hotTag in platformMusicTag.HotTags)
        {
            DiscoverTags.Add(new DiscoverTagViewModel()
            {
                Id = hotTag.Id,
                Name = hotTag.Name,
                IsSelected = false
            });
        }
        DiscoverTags.Add(new DiscoverTagViewModel()
        {
            Id = "选择分类",
            Name = "选择分类",
            IsSelected = false
        });
    }
    private async Task SelectMusicTag(string id)
    {
        //TODO 有空了重新设计一下这里的实现，现在有点凌乱（榜单、选择分类、热门标签）
        if (id == "选择分类")
        {
            var selectedId = await GetSelectedTagId();
            if (selectedId.IsEmpty())
            {
                return;
            }
            id = selectedId;
        }

        foreach (var tag in DiscoverTags)
        {
            if (tag.Id == id)
            {
                tag.IsSelected = true;
            }
            else
            {
                tag.IsSelected = false;
            }
        }

        SongMenus.Clear();
        _currentPage = 1;
        try
        {
            Loading("加载中....");
            List<SongMenu> songMenus;
            SongMenuEnum songMenuType;
            if (id == "榜单")
            {
                _currentTagId = "";
                songMenus = await _musicNetworkService.GetSongMenusFromTop(Platform);
                songMenuType = SongMenuEnum.Top;
            }
            else
            {
                _currentTagId = id;
                songMenus = await _musicNetworkService.GetSongMenusFromTagAsync(Platform, id, _currentPage);
                songMenuType = SongMenuEnum.Tag;
            }

            foreach (var songMenu in songMenus)
            {
                SongMenus.Add(new SongMenuViewModel()
                {
                    SongMenuType = songMenuType,
                    PlatformName = Platform.GetDescription(),
                    Id = songMenu.Id,
                    Name = songMenu.Name,
                    ImageUrl = songMenu.ImageUrl,
                    LinkUrl = songMenu.LinkUrl
                });
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"歌单加载失败：{Platform.GetDescription()},id={id}", ex);
        }
        finally
        {
            LoadComplete();
        }
    }

    [RelayCommand]
    private async void LoadLastPageTagSongMenusAsync()
    {
        if (_currentTagId.IsEmpty())
        {
            return;
        }

        try
        {
            Loading("加载中....");
            var page = _currentPage + 1;
            var songMenus = await _musicNetworkService.GetSongMenusFromTagAsync(Platform, _currentTagId, page);
            _currentPage = page;
            foreach (var songMenu in songMenus)
            {
                SongMenus.Add(new SongMenuViewModel()
                {
                    SongMenuType = SongMenuEnum.Tag,
                    PlatformName = Platform.GetDescription(),
                    Id = songMenu.Id,
                    Name = songMenu.Name,
                    ImageUrl = songMenu.ImageUrl,
                    LinkUrl = songMenu.LinkUrl
                });
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"歌单滚动加载失败：{Platform.GetDescription()},id={_currentTagId}", ex);
        }
        finally
        {
            LoadComplete();
        }
    }

    private async Task<string> GetSelectedTagId()
    {
        if (!PlatformMusicTags.TryGetValue(Platform, out var platformMusicTag))
        {
            return "";
        }
        var popup = new ChooseTagPage(platformMusicTag.AllTypes);
        var result = await App.Current.MainPage.ShowPopupAsync(popup);
        if (result == null)
        {
            return "";
        }
        return result.ToString();
    }

    [RelayCommand]
    private async void GotoSongMenuPageAsync(SongMenuViewModel songMenu)
    {
        string json = HttpUtility.UrlEncode(songMenu.ToJson());
        await Shell.Current.GoToAsync($"{nameof(SongMenuPage)}?Json={json}&PlatformString={Platform}");
    }

    [RelayCommand]
    private async void GoToSearchPageAsync()
    {
        await App.Current.MainPage.Navigation.PushAsync(_searchPage, true);
    }
}