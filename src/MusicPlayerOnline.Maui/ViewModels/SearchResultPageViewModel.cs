using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Business.Factories;
using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Maui;
using MusicPlayerOnline.Maui.Services;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MusicPlayerOnline.Maui.ViewModels
{
    [QueryProperty(nameof(SearchKeyword), nameof(SearchKeyword))]
    public class SearchResultPageViewModel : ViewModelBase
    {
        private readonly IMusicNetworkService _searchService;
        private readonly IMusicService _musicService;
        private readonly IPlaylistService _playlistService;

        private string _lastSearchKeyword = "";
        public Command<SearchResultViewModel> AddToMyFavoriteCommand => new Command<SearchResultViewModel>(AddToMyFavorite);
        public Command SelectedChangedCommand => new Command(SearchFinished);

        public ICommand SearchCommand => new Command(Search);

        public SearchResultPageViewModel(IMusicNetworkService searchService, IMusicServiceFactory musicServiceFactory, IPlaylistServiceFactory playlistServiceFactory)
        {
            MusicSearchResult = new ObservableCollection<SearchResultViewModel>();

            _searchService = searchService;
            _musicService = musicServiceFactory.Create();
            _playlistService = playlistServiceFactory.Create();
        }

        private string _title;
        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _textToSearch;
        /// <summary>
        /// 搜索
        /// </summary>
        public string TextToSearch
        {
            get => _textToSearch;
            set
            {
                _textToSearch = value;
                OnPropertyChanged();
            }
        }


        public IEnumerable<PlatformEnum> MyEnumTypeValues
        {
            get
            {
                return System.Enum.GetValues(typeof(PlatformEnum)).Cast<PlatformEnum>();
            }
        }

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

        private bool _isMusicSearching;
        /// <summary>
        /// 正在搜索歌曲
        /// </summary>
        public bool IsMusicSearching
        {
            get => _isMusicSearching;
            set
            {
                _isMusicSearching = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SearchResultViewModel> _musicSearchResult;
        /// <summary>
        /// 搜索到的结果列表
        /// </summary>
        public ObservableCollection<SearchResultViewModel> MusicSearchResult
        {
            get => _musicSearchResult;
            set
            {
                _musicSearchResult = value;
                OnPropertyChanged();
            }
        }

        private SearchResultViewModel _musicSelectedResult;
        /// <summary>
        /// 选择的结果集
        /// </summary>
        public SearchResultViewModel MusicSelectedResult
        {
            get => _musicSelectedResult;
            set
            {
                _musicSelectedResult = value;
                OnPropertyChanged();
            }
        }
        private async void Search()
        {
            if (SearchKeyword.IsEmpty())
            {
                return;
            }

            if (SearchKeyword == _lastSearchKeyword)
            {
                return;
            }
            _lastSearchKeyword = SearchKeyword;

            try
            {
                IsMusicSearching = true;
                Title = $"搜索: {SearchKeyword}";
                MusicSearchResult.Clear();
                var musics = await _searchService.Search(GlobalConfig.MyUserSetting.Search.EnablePlatform, SearchKeyword);
                if (musics.Count == 0)
                {
                    DependencyService.Get<IToastService>().Show("哦吼，啥也没有搜到");
                    return;
                }

                foreach (var musicInfo in musics)
                {
                    if (GlobalConfig.MyUserSetting.Search.IsHideShortMusic && musicInfo.Duration != 0 && musicInfo.Duration <= 60 * 1000)
                    {
                        continue;
                    }

                    MusicSearchResult.Add(new SearchResultViewModel()
                    {
                        Platform = musicInfo.Platform.GetDescription(),
                        Name = musicInfo.Name,
                        Alias = musicInfo.Alias == "" ? "" : $"（{musicInfo.Alias}）",
                        Artist = musicInfo.Artist,
                        Album = musicInfo.Album,
                        Duration = musicInfo.DurationText,
                        SourceData = musicInfo
                    });
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToastService>().Show("抱歉，网络可能出小差了~");
            }
            finally
            {
                IsMusicSearching = false;
            }
        }

        private async void AddToMyFavorite(SearchResultViewModel searchResult)
        {
            Music music;

            bool succeed;
            string message;
            (succeed, message, music) = await SaveMusic(searchResult.SourceData);
            if (succeed == false)
            {
                if (GlobalConfig.MyUserSetting.Search.IsCloseSearchPageWhenPlayFailed)
                {
                    await Shell.Current.GoToAsync("..", true);
                }
                DependencyService.Get<IToastService>().Show(message);
                return;
            }
            //TODO 重构，是否需要广播
            //MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
            //TODO 页面跳转
            //await Shell.Current.GoToAsync($"{nameof(AddToMyFavoritePage)}?{nameof(AddToMyFavoritePageViewModel.AddedMusicId)}={music.Id}", true);
            //TODO 播放音乐
            //await GlobalMethods.PlayMusic(music);
        }

        private async void SearchFinished()
        {
            Music music;

            bool succeed;
            string message;

            (succeed, message, music) = await SaveMusic(MusicSelectedResult.SourceData);
            if (succeed == false)
            {
                if (GlobalConfig.MyUserSetting.Search.IsCloseSearchPageWhenPlayFailed)
                {
                    await Shell.Current.GoToAsync("..", true);
                }
                DependencyService.Get<IToastService>().Show(message);
                return;
            }

            //TODO 重构逻辑
            //if (await GlobalMethods.PlayMusic(music) == false)
            //{
            //    return;
            //}
            //MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
            //await Shell.Current.GoToAsync($"..", false);
            //await Shell.Current.GoToAsync($"//{nameof(PlayingPage)}", true);
        }

        private async Task<(bool Succeed, string Message, Music MusicDetailResult)> SaveMusic(MusicSearchResult searchResult)
        {
            var music = await _searchService.GetMusicDetail(searchResult);
            if (music == null)
            {
                return (false, "emm没有解析出歌曲信息", null);
            }

            await _musicService.AddOrUpdateAsync(music);
            await _playlistService.AddToPlaylist(music);

            return (true, "", music);
        }
    }
}
