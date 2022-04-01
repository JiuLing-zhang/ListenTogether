namespace MusicPlayerOnline.Maui.ViewModels
{
    /// <summary>
    /// 歌词详情
    /// </summary>
    public class LyricViewModel : ViewModelBase
    {
        private int _position;
        /// <summary>
        /// 歌词位置
        /// </summary>
        public int Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        private string _info;
        /// <summary>
        /// 歌词
        /// </summary>
        public string Info
        {
            get => _info;
            set
            {
                _info = value;
                OnPropertyChanged();
            }
        }

        private bool _isHighlight = false;
        /// <summary>
        /// 是否高亮显示
        /// </summary>
        public bool IsHighlight
        {
            get => _isHighlight;
            set
            {
                _isHighlight = value;
                OnPropertyChanged();
            }
        }
    }
}
