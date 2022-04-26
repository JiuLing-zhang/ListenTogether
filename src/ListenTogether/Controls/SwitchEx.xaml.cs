namespace MusicPlayerOnline.Maui.Controls;

public partial class SwitchEx : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(SwitchEx),
            string.Empty);
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty IsToggledProperty =
        BindableProperty.Create(
            nameof(IsToggled),
            typeof(bool),
            typeof(Switch),
            false,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsToggled
    {
        get { return (bool)GetValue(IsToggledProperty); }
        set { SetValue(IsToggledProperty, value); }
    }

    public SwitchEx()
    {
        InitializeComponent();
    }

}