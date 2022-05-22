namespace ListenTogether.Handlers.GaussianBlurHandler;
internal class GaussianImage : Image, IGaussianImage
{
    public static readonly BindableProperty UrlProperty =
        BindableProperty.Create(
            nameof(Url),
            typeof(string),
            typeof(GaussianImage),
            string.Empty);
    public string Url
    {
        get { return (string)GetValue(UrlProperty); }
        set { SetValue(UrlProperty, value); }
    }
}