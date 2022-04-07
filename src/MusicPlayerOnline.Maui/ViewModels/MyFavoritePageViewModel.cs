using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels
{
    public class MyFavoritePageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;
        private readonly IPlaylistService _playlistService;
        private readonly IMusicService _musicService;
        public ICommand MyFavoriteAddCommand => new Command(AddMyFavorite);
        public ICommand SelectedChangedCommand => new Command<MyFavoriteViewModel>(SelectedChangedDo);
        public ICommand PlayAllMusicsCommand => new Command<MyFavoriteViewModel>(PlayAllMusics);
        public string Title => "我的歌单";
        public MyFavoritePageViewModel(IMyFavoriteServiceFactory myFavoriteServiceFactory, IPlaylistServiceFactory playlistServiceFactory, IMusicServiceFactory musicServiceFactory)
        {
            FavoriteList = new ObservableCollection<MyFavoriteViewModel>();

            _myFavoriteService = myFavoriteServiceFactory.Create();
            _playlistService = playlistServiceFactory.Create();
            _musicService = musicServiceFactory.Create();
        }

        public async Task InitializeAsync()
        {
            if (FavoriteList.Count > 0)
            {
                FavoriteList.Clear();
            }

            var myFavoriteList = await _myFavoriteService.GetAllAsync();
            foreach (var myFavorite in myFavoriteList)
            {
                FavoriteList.Add(new MyFavoriteViewModel()
                {
                    Id = myFavorite.Id,
                    Name = myFavorite.Name,
                    MusicCount = myFavorite.MusicCount,
                    ImageUrl = myFavorite.ImageUrl
                });
            }
        }

        private ObservableCollection<MyFavoriteViewModel> _favoriteList;
        public ObservableCollection<MyFavoriteViewModel> FavoriteList
        {
            get => _favoriteList;
            set
            {
                _favoriteList = value;
                OnPropertyChanged();
            }
        }

        private async void AddMyFavorite()
        {
            await Shell.Current.GoToAsync($"{nameof(MyFavoriteAddPage)}", true);
        }

        private async void SelectedChangedDo(MyFavoriteViewModel selected)
        {
            await Shell.Current.GoToAsync($"{nameof(MyFavoriteDetailPage)}?{nameof(MyFavoriteDetailPageViewModel.MyFavoriteId)}={selected.Id}", true);
        }

        private async void PlayAllMusics(MyFavoriteViewModel myFavorite)
        {
            List<MyFavoriteDetail> myFavoriteMusics = await _myFavoriteService.GetMyFavoriteDetail(myFavorite.Id);
            if (myFavoriteMusics == null)
            {
                ToastService.Show("播放失败：播放列表是空哒~~~");
                return;
            }

            if (GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlayMyFavorite)
            {
                await _playlistService.RemoveAllAsync();
            }

            int index = 0;
            foreach (var myFavoriteMusic in myFavoriteMusics)
            {
                var playlist = new Playlist()
                {
                    MusicId = myFavoriteMusic.MusicId,
                    MusicName = myFavoriteMusic.MusicName,
                    MusicArtist = myFavoriteMusic.MusicArtist
                };

                await _playlistService.AddToPlaylist(playlist);
                if (index == 0)
                {
                    //TODO 播放
                    //if (await GlobalMethods.PlayMusic(music) == false)
                    //{
                    //    return;
                    //}
                }
                index++;
            }

            //TODO 页面跳转
            //await Shell.Current.GoToAsync($"//{nameof(PlayingPage)}", true);         
        }
    }
}
