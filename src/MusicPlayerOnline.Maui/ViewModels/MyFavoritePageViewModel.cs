using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels
{
    public class MyFavoritePageViewModel : ViewModelBase
    {
        readonly IServiceProvider _services;
        private IMyFavoriteService _myFavoriteService;
        private IPlaylistService _playlistService;
        private IMusicService _musicService;
        public ICommand MyFavoriteAddCommand => new Command(AddMyFavorite);
        public ICommand EnterMyFavoriteDetailCommand => new Command<MyFavoriteViewModel>(EnterMyFavoriteDetail);
        public ICommand PlayAllMusicsCommand => new Command<MyFavoriteViewModel>(PlayAllMusics);
        public string Title => "我的歌单";
        public MyFavoritePageViewModel(IServiceProvider services)
        {
            FavoriteList = new ObservableCollection<MyFavoriteViewModel>();
            _services = services;
        }

        public async Task InitializeAsync()
        {
            _myFavoriteService = _services.GetService<IMyFavoriteServiceFactory>().Create();
            _playlistService = _services.GetService<IPlaylistServiceFactory>().Create();
            _musicService = _services.GetService<IMusicServiceFactory>().Create();

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

        private async void EnterMyFavoriteDetail(MyFavoriteViewModel selected)
        {
            if (selected.MusicCount == 0)
            {
                ToastService.Show("当前歌单是空的哦");
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(MyFavoriteDetailPage)}?{nameof(MyFavoriteDetailPageViewModel.MyFavoriteId)}={selected.Id}", true);
        }

        private async void PlayAllMusics(MyFavoriteViewModel selected)
        {
            if (selected.MusicCount == 0)
            {
                ToastService.Show("当前歌单是空的哦");
                return;
            }

            var myFavoriteMusics = await _myFavoriteService.GetMyFavoriteDetail(selected.Id);
            if (myFavoriteMusics == null)
            {
                ToastService.Show("播放失败：没有查询到歌单信息~~~");
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
