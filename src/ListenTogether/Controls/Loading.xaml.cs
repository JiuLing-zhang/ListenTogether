namespace ListenTogether.Controls;

public partial class Loading : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(Loading),
            string.Empty);
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty IsRunningProperty =
        BindableProperty.Create(
            nameof(IsRunning),
            typeof(bool),
            typeof(Loading),
            false);

    public bool IsRunning
    {
        get { return (bool)GetValue(IsRunningProperty); }
        set { SetValue(IsRunningProperty, value); }
    }

    public Loading()
    {
        InitializeComponent();
    }
}