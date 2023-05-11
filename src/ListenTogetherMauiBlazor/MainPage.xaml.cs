using Microsoft.UI.Xaml;

namespace ListenTogetherMauiBlazor
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            SetWindowSize();
        }

        private void SetWindowSize()
        {
            //16:9 比例系数 = 0.5625
            //屏幕宽度 = 设备像素宽度 / 缩放比例
            //窗口宽度 = 屏幕宽度 * 0.5625
            //屏幕高度 = 设备像素高度 / 缩放比例
            //窗口高度 = 窗口高度 * 0.5625
            var disp = DeviceDisplay.Current.MainDisplayInfo;

            var screenWidth = disp.Width / disp.Density;
            var screenHeight = disp.Height / disp.Density;

            var width = screenWidth * 0.6;
            //设置窗口的最小宽度
            if (width < 1000)
            {
                width = 1000;
            }
            var height = width * 0.5625;

            //居中
            Window.X = (screenWidth - width) / 2;
            Window.Y = (screenHeight - height) / 2;

            //设置窗口大小
            Window.Width = width;
            Window.Height = height;
            // Window.MinimumWidth = width;
            //  Window.MinimumHeight = height;
            //  Window.MaximumWidth = width;
            //  Window.MaximumHeight = height;
        }
    }
}