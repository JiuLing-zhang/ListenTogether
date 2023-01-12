using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogether.ViewModels;
public class MiGuPageViewModel
{
    private readonly IMusicNetworkService _musicNetworkService;
    public MiGuPageViewModel(IMusicNetworkService musicNetworkService)
    {
        _musicNetworkService = musicNetworkService;
    }
    public async Task InitializeAsync()
    {
        await _musicNetworkService.GetMusicTagsAsync(Model.Enums.PlatformEnum.MiGu);
    }
}