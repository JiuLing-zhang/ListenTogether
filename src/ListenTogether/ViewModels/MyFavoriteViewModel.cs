using JiuLing.CommonLibs.ExtensionMethods;

namespace ListenTogether.ViewModels
{
    public class MyFavoriteViewModel : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged("ImageUrl");

                _imageByteArray = ImageCacheUtils.GetByteArrayUsingCache(value);
                OnPropertyChanged("ImageByteArray");
            }
        }

        private byte[] _imageByteArray;
        public byte[] ImageByteArray
        {
            get => _imageByteArray;
        }


        private int _musicCount;
        public int MusicCount
        {
            get => _musicCount;
            set
            {
                _musicCount = value;
                OnPropertyChanged();
            }
        }
    }
}
