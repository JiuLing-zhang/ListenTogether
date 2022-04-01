using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Business.Factories;
using MusicPlayerOnline.Business.Interfaces;
using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels
{
    public class PlaylistPageViewModel : ViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly IPlaylistService _playlistService;

        public Command<MusicViewModel> AddToMyFavoriteCommand => new Command<MusicViewModel>(AddToMyFavorite);
        public Command ClearPlaylistCommand => new Command(ClearPlaylist);
        public PlaylistPageViewModel(IMusicServiceFactory musicServiceFactory, IPlaylistServiceFactory playlistServiceFactory)
        {
            Playlist = new ObservableCollection<MusicViewModel>();

            _playlistService = playlistServiceFactory.Create();
            _musicService = musicServiceFactory.Create();

            //TODO 更新播放列表
            //MessagingCenter.Subscribe<SearchResultPageViewModel>(this, SubscribeKey.UpdatePlaylist, (_) =>
            //{
            //    GetPlaylist();
            //});
            //MessagingCenter.Subscribe<MyFavoriteDetailPageViewModel>(this, SubscribeKey.UpdatePlaylist, (_) =>
            //{
            //    GetPlaylist();
            //});
            GetPlaylist();
        }
        public void OnAppearing()
        {
            if (SearchKeyword.IsNotEmpty())
            {
                SearchKeyword = "";
            }
        }
        private async void GetPlaylist()
        {
            if (Playlist.Count > 0)
            {
                Playlist.Clear();
            }
            var playlist = await _playlistService.GetAllAsync();
            foreach (var item in playlist)
            {
                Playlist.Add(new MusicViewModel()
                {
                    Id = item.MusicId,
                    Name = item.MusicName,
                    Artist = item.MusicArtist
                });
            }

        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "播放列表";

        private string _searchKeyword;
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MusicViewModel> _playlist;
        /// <summary>
        /// 搜索到的结果列表
        /// </summary>
        public ObservableCollection<MusicViewModel> Playlist
        {
            get => _playlist;
            set
            {
                _playlist = value;
                OnPropertyChanged();
            }
        }

        private MusicViewModel _selectedItem;
        public MusicViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();

                SelectedChangedDo();
            }
        }

        public async void Search()
        {
            if (SearchKeyword.IsEmpty())
            {
                return;
            }

            //TODO 页面跳转
            //await Shell.Current.GoToAsync($"{nameof(SearchResultPage)}?{nameof(SearchResultPageViewModel.SearchKeyword)}={SearchKeyword}", true);
        }

        private async void SelectedChangedDo()
        {
            if (SelectedItem == null)
            {
                return;
            }
            var music = await _musicService.GetOneAsync(SelectedItem.Id);
            if (music == null)
            {
                ToastService.Show("获取歌曲信息失败");
                return;
            }

            //TODO 播放并跳转页面
            //if (await GlobalMethods.PlayMusic(music) == false)
            //{
            //    return;
            //}
            //await Shell.Current.GoToAsync($"//{nameof(PlayingPage)}", true);
        }

        private async void AddToMyFavorite(MusicViewModel music)
        {
            if (music == null)
            {
                return;
            }
            //TODO 跳转页面
            //await Shell.Current.GoToAsync($"{nameof(AddToMyFavoritePage)}?{nameof(AddToMyFavoritePageViewModel.AddedMusicId)}={music.Id}", true);
        }

        public async void RemovePlaylistItem(MusicViewModel music)
        {
            if (music == null)
            {
                return;
            }

            //TODO 删除一条
            //await _playlistService.RemoveAsync(music.Id);
            GetPlaylist();
        }

        private async void ClearPlaylist()
        {
            if (Playlist.Count == 0)
            {
                ToastService.Show("别删除了，播放列表是空哒");
                return;
            }

            var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除播放列表吗？", "确定", "取消");
            if (isOk == false)
            {
                return;
            }
            await _playlistService.RemoveAllAsync();
            ToastService.Show("播放列表已删除");
            GetPlaylist();
        }
    }
}
