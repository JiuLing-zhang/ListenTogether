using ListenTogether.ViewModels;
namespace ListenTogether.Pages;
public partial class DesktopShell
{
    public DesktopShell()
    {
        InitializeComponent();

        BindingContext = new ShellViewModel();
    }
}