using MudBlazor;

namespace ListenTogether.Pages;
public class DialogBuilder
{
    public static DialogParameters DialogParameters(string text)
    {
        var parameters = new DialogParameters();
        parameters.Add("Content", text);
        return parameters;
    }
}