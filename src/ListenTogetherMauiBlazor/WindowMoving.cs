using ListenTogether.Pages;
using Windows.Win32;

namespace ListenTogetherMauiBlazor;
internal class WindowMoving : IWindowMoving
{
    private bool _isMoving;

    private double _mouseStartX;
    private double _mouseStartY;
    private double _windowStartLeft;
    private double _windowStartTop;

    public WindowMoving()
    {
        IDispatcherTimer timer = App.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(10);
        timer.Tick += (_, __) =>
        {
            if (!_isMoving)
            {
                return;
            }
            PInvoke.GetCursorPos(out var point);
            var mauiWindow = App.Current.Windows.First();
            mauiWindow.X = _windowStartLeft - _mouseStartX + point.X;
            mauiWindow.Y = _windowStartTop - _mouseStartY + point.Y;
        };
        timer.Start();
    }

    public void MouseDown()
    {
        PInvoke.GetCursorPos(out var point);
        _mouseStartX = point.X;
        _mouseStartY = point.Y;
        var mauiWindow = App.Current.Windows.First();
        _windowStartLeft = mauiWindow.X;
        _windowStartTop = mauiWindow.Y;
        _isMoving = true;
    }

    public void MouseUp()
    {
        _isMoving = false;
    }
}