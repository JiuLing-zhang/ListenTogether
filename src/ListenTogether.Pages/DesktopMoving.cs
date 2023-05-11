namespace ListenTogether.Pages;
public class DesktopMoving
{
    public event EventHandler<EventArgs>? OnMouseDown;
    public event EventHandler<EventArgs>? OnMouseUp;
    public void MouseDown()
    {
        OnMouseDown?.Invoke(this, EventArgs.Empty);
    }
    public void MouseUp()
    {
        OnMouseUp?.Invoke(this, EventArgs.Empty);
    }
}