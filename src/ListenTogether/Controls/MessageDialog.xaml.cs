using CommunityToolkit.Maui.Views;

namespace ListenTogether.Controls;

public partial class MessageDialog : Popup
{
    public MessageDialog(string message)
    {
        InitializeComponent();
        LblMessage.Text = message;
    }

    private void BtnClose_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}