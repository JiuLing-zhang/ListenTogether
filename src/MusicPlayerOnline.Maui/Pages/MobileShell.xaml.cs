using MusicPlayerOnline.Maui.ViewModels;
namespace MusicPlayerOnline.Maui.Pages;
public partial class MobileShell
{
    public MobileShell()
    {
        InitializeComponent();

        BindingContext = new ShellViewModel();
    }
}