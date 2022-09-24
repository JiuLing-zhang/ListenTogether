using ListenTogether.ViewModels;
namespace ListenTogether.Pages;
public partial class DesktopShell
{
    public DesktopShell()
    {
        InitializeComponent();

        BindingContext = new ShellViewModel();

        SetWindowSize();
    }

    private void SetWindowSize()
    {
        Task.Run(() =>
        {
            while (true)
            {
                //等待窗口初始化完成
                if (Window == null || DeviceDisplay.Current.MainDisplayInfo.Width == 0)
                {
                    continue;
                }

                //16:9 比例系数 = 0.5625
                //屏幕宽度 = 设备像素宽度 / 缩放比例
                //窗口宽度 = 屏幕宽度 * 0.5625
                //屏幕高度 = 设备像素高度 / 缩放比例
                //窗口高度 = 窗口高度 * 0.5625
                var disp = DeviceDisplay.Current.MainDisplayInfo;

                var screenWidth = disp.Width / disp.Density;
                var screenHeight = disp.Height / disp.Density;

                var width = screenWidth * 0.6;
                var height = width * 0.5625;

                //居中
                Window.X = (screenWidth - width) / 2;
                Window.Y = (screenHeight - height) / 2;

                //设置窗口大小
                Window.Width = width;
                Window.Height = height;
                Window.MinimumWidth = width;
                Window.MinimumHeight = height;
                return;
            }

        });
    }
}