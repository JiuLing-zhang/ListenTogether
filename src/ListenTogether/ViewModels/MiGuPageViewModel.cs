using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace ListenTogether.ViewModels;
public partial class MiGuPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<MusicTagViewModel>? _hotTags = null;

    private readonly IMusicNetworkService _musicNetworkService;

    public MiGuPageViewModel(IMusicNetworkService musicNetworkService)
    {
        _musicNetworkService = musicNetworkService;
    }
    public async Task InitializeAsync()
    {
        if (HotTags == null)
        {
            var (hotTags, allTypes) = await _musicNetworkService.GetMusicTagsAsync(Model.Enums.PlatformEnum.MiGu);
            HotTags = new ObservableCollection<MusicTagViewModel>();
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

            await GetDetailAsync("榜单");
        }
    }

    private async Task GetDetailAsync(string tagId)
    {
        for (int i = 0; i < HotTags.Count - 1; i++)
        {
            HotTags[i].BackgroundColor = HotTags[i].Id == tagId ? "#C98FFF" : "Transparent";
        }
    }

    [RelayCommand]
    private async void GetTagDetailAsync()
    {

    }
    
}