using CommunityToolkit.Mvvm.Input;

namespace ListenTogether.ViewModels;
public partial class MusicResultViewModel : ViewModelBase
{

    [RelayCommand]
    public async void PlayMusicAsync(string id)
    {
        throw new NotImplementedException("播放");
    }

    [RelayCommand]
    public async void AddToMyFavoriteAsync(string id)
    {
        throw new NotImplementedException("添加到收藏");
    }

}