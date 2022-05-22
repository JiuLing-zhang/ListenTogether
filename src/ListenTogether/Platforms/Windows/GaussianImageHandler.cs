using ListenTogether.Platforms.Windows;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Media.Imaging;

namespace ListenTogether.Handlers.GaussianBlurHandler;

internal partial class GaussianImageHandler : ViewHandler<IGaussianImage, Microsoft.UI.Xaml.Controls.Image>
{
    private static HttpClient _httpClient = new HttpClient();
    protected override Microsoft.UI.Xaml.Controls.Image CreatePlatformView()
    {
        return new Microsoft.UI.Xaml.Controls.Image()
        {
            //图片适配方式直接使用UniformToFill，如果后期需要在修改为自定义属性
            Stretch = Microsoft.UI.Xaml.Media.Stretch.UniformToFill
        };
    }

    static async void MapUrl(GaussianImageHandler handler, IGaussianImage gaussianBlurImage)
    {
        if (gaussianBlurImage.Url.IsEmpty())
        {
            return;
        }
        var buffer = await _httpClient.GetByteArrayAsync(gaussianBlurImage.Url);
        var gaussianBlur = new GaussianBlur(buffer);
        var newBuffer = gaussianBlur.Process(25);

        var memStream = new MemoryStream(newBuffer);
        var bitmap = new BitmapImage();
        bitmap.SetSource(memStream.AsRandomAccessStream());
        handler.PlatformView.Source = bitmap;
    }
}