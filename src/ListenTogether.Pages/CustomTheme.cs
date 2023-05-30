using MudBlazor;

namespace ListenTogether.Pages;
public class CustomTheme
{
    public MudTheme Theme
    {
        get
        {
            return new MudTheme()
            {
                Palette = new PaletteLight()
                {
                    Primary = "#C98FFF",
                    Secondary = Colors.Green.Accent4,
                    AppbarBackground = Colors.Red.Default,
                    TableStriped = "#F5F5F5",
                    TableHover = "#F1F6FD",
                    DarkLighten = "#CACDD1"

                },
                PaletteDark = new PaletteDark()
                {
                    Primary = "#C98FFF",
                    Background = "#333333",
                    Surface = "#333333",
                    TableStriped = "#292929",
                    TableHover = "#182437",
                    DarkLighten = "#494C50",
                    Black = "#858585",
                    TextPrimary = "#FFFFFF",
                    OverlayDark = "#7575757A"
                }
            };
        }
    }
}