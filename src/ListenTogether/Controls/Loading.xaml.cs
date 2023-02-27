using CommunityToolkit.Maui.Views;

namespace ListenTogether.Controls;

public partial class LoadingPage : Popup
{
    public LoadingPage(string message)
    {
        InitializeComponent();

        double width = App.Current?.MainPage?.Window.Width ?? 300;
        double height = App.Current?.MainPage?.Window.Height ?? 150;
        this.Size = new Size(width, height);
        LblMessage.Text = message;
    }
}