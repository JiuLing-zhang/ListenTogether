using CommunityToolkit.Maui.Views;

namespace ListenTogether.Controls;

public partial class LoadingPage : Popup
{
    public LoadingPage(string message)
    {
        InitializeComponent();
        LblMessage.Text = message;
    }
}