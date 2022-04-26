using ListenTogether.ViewModels;
namespace ListenTogether.Pages;
public partial class MobileShell
{
    public MobileShell()
    {
        InitializeComponent();

        BindingContext = new ShellViewModel();
    }
}