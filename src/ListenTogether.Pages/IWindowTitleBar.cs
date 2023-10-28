namespace ListenTogether.Pages;
public interface IWindowTitleBar
{
    public bool IsMaximized { get; }
    void Minimize();
    void Maximize();
    void Close();
    void SetTitle(string title);
}