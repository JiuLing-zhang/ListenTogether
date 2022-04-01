using MusicPlayerOnline.Business.Factories;
using MusicPlayerOnline.Business.Interfaces;
using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels
{
    public class MyFavoritePageViewModel : ViewModelBase
    {
        private readonly IMyFavoriteService _myFavoriteService;

        public Command AddMyFavoriteCommand => new Command(AddMyFavorite);
        public Command SelectedChangedCommand => new Command(SelectedChangedDo);
        public Command<MyFavoriteViewModel> EditFavoriteCommand => new Command<MyFavoriteViewModel>(EditFavorite);

        public string Title => "我的歌单";
        public MyFavoritePageViewModel(IMyFavoriteServiceFactory myFavoriteServiceFactory)
        {
            FavoriteList = new ObservableCollection<MyFavoriteViewModel>();

            _myFavoriteService = myFavoriteServiceFactory.Create();
        }

        public async void OnAppearing()
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

        private MyFavoriteViewModel _selectedResult;
        public MyFavoriteViewModel SelectedResult
        {
            get => _selectedResult;
            set
            {
                _selectedResult = value;
                OnPropertyChanged();
            }
        }

        private async void AddMyFavorite()
        {
            //TODO 页面跳转
            //await Shell.Current.GoToAsync(nameof(AddMyFavoritePage), true);
        }

        private async void SelectedChangedDo()
        {
            if (SelectedResult.MusicCount == 0)
            {
                ToastService.Show("该歌单是空的");
                return;
            }
            //TODO 页面跳转
            //await Shell.Current.GoToAsync($"{nameof(MyFavoriteDetailPage)}?{nameof(MyFavoriteDetailPageViewModel.MyFavoriteId)}={SelectedResult.Id}", true);
        }

        private async void EditFavorite(MyFavoriteViewModel myFavorite)
        {
            //TODO 页面跳转
            //await Shell.Current.GoToAsync($"{nameof(EditMyFavoritePage)}?{nameof(EditMyFavoritePageViewModel.MyFavoriteId)}={myFavorite.Id}", true);
        }
    }
}
