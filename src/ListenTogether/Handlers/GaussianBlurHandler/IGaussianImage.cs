using IImage = Microsoft.Maui.IImage;

namespace ListenTogether.Handlers.GaussianBlurHandler;
internal interface IGaussianImage : IImage
{
    string Url { get; set; }
}