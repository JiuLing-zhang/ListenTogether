namespace ListenTogether.Pages;
public class DesktopNotification
{
    public event EventHandler<string>? OnSetTitle;
    public event EventHandler<EventArgs>? OnWindowMinimize;
    public event EventHandler<EventArgs>? OnWindowMaximize;
    public event EventHandler<EventArgs>? OnWindowShowNormal;
    public event EventHandler<EventArgs>? OnWindowClose;

    public void SetTitle(string title)
    {
        OnSetTitle?.Invoke(this, title);
    }

    public void MinimizeWindow()
    {
        OnWindowMinimize?.Invoke(this, EventArgs.Empty);
    }
    public void MaximizeWindow()
    {
        OnWindowMaximize?.Invoke(this, EventArgs.Empty);
    }
    public void ShowNormalWindow()
    {
        OnWindowShowNormal?.Invoke(this, EventArgs.Empty);
    }
    public void CloseWindow()
    {
        OnWindowClose?.Invoke(this, EventArgs.Empty);
    }
}

public class ThresholdReachedEventArgs : EventArgs
{
    public int Threshold { get; set; }
    public DateTime TimeReached { get; set; }
}