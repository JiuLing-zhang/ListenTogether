using MusicPlayerOnline.Maui.ViewModels;
namespace MusicPlayerOnline.Maui.Pages;
public partial class DesktopShell
{
    public DesktopShell()
    {
        InitializeComponent();

        BindingContext = new ShellViewModel();
    }
}