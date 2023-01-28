using CommunityToolkit.Maui.Views;

namespace ListenTogether.Pages;

public partial class ChooseMyFavoritePage : Popup
{
    public ChooseMyFavoritePage()
    {
        InitializeComponent();
    }

    void OnYesButtonClicked(object? sender, EventArgs e) => Close(true);

    void OnNoButtonClicked(object? sender, EventArgs e) => Close(false);
}