using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels
{
    public class MyFavoritePageViewModel : ViewModelBase
    {
        readonly IServiceProvider _services;
        private IMyFavoriteService _myFavoriteService;
        private IPlaylistService _playlistService;
        private IMusicService _musicService;
        private PlayerService _playerService;
        public ICommand MyFavoriteAddCommand => new Command(AddMyFavorite);
        public ICommand EnterMyFavoriteDetailCommand => new Command<MyFavoriteViewModel>(EnterMyFavoriteDetail);
        public ICommand PlayAllMusicsCommand => new Command<MyFavoriteViewModel>(PlayAllMusics);
        public string Title => "我的歌单";
        public MyFavoritePageViewModel(IServiceProvider services, PlayerService playerService)
        {
            FavoriteList = new ObservableCollection<MyFavoriteViewModel>();
            _services = services;
            _playerService = playerService;
        }

        public async Task InitializeAsync()
        {
            try
            {
                IsBusy = true;
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
            catch (Exception ex)
            {
                await ToastService.Show("我的歌单加载失败");
                Logger.Error("我的歌单页面初始化失败。", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsNotBusy");
            }
        }
        public bool IsNotBusy => !_isBusy;

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
            string myFavoriteName = await App.Current.MainPage.DisplayPromptAsync("添加歌单", "请输入歌单名称：", "添加", "取消");
            if (myFavoriteName.IsEmpty())
            {
                return;
            }

            try
            {
                IsBusy = true;
                if (await _myFavoriteService.NameExist(myFavoriteName))
                {
                    await ToastService.Show("歌单名称已存在");
                    return;
                }

                var myFavorite = new MyFavorite()
                {
                    Name = myFavoriteName,
                    MusicCount = 0
                };
                var newMyFavorite = await _myFavoriteService.AddOrUpdateAsync(myFavorite);
                if (newMyFavorite == null)
                {
                    await ToastService.Show("添加失败");
                    return;
                }
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                await ToastService.Show("添加失败，网络出小差了");
                Logger.Error("歌单添加失败。", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void EnterMyFavoriteDetail(MyFavoriteViewModel selected)
        {
            if (selected.MusicCount == 0)
            {
                await ToastService.Show("当前歌单是空的哦");
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(MyFavoriteDetailPage)}?{nameof(MyFavoriteDetailPageViewModel.MyFavoriteId)}={selected.Id}", true);
        }

        private async void PlayAllMusics(MyFavoriteViewModel selected)
        {
            if (selected.MusicCount == 0)
            {
                await ToastService.Show("当前歌单是空的哦");
                return;
            }

            var myFavoriteMusics = await _myFavoriteService.GetMyFavoriteDetail(selected.Id);
            if (myFavoriteMusics == null)
            {
                await ToastService.Show("播放失败：没有查询到歌单信息~~~");
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
                    var music = await _musicService.GetOneAsync(myFavoriteMusic.MusicId);
                    await _playerService.PlayAsync(music);
                }
                index++;
            }
        }
    }
}
