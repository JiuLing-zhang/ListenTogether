using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Media.Imaging;

namespace ListenTogether.Handlers.GaussianImage;
/// <summary>
/// 图像高斯模糊处理程序
/// </summary>
public partial class GaussianImageHandler : ViewHandler<IGaussianImage, Microsoft.UI.Xaml.Controls.Image>
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

    static async void MapSourceByteArray(GaussianImageHandler handler, IGaussianImage gaussianBlurImage)
    {
        if (gaussianBlurImage.SourceByteArray == null)
        {
            return;
        }

        var gaussianBlur = new GaussianBlur(gaussianBlurImage.SourceByteArray);
        var newBuffer = gaussianBlur.Process(100);

        var memStream = new MemoryStream(newBuffer);
        var bitmap = new BitmapImage();
        bitmap.SetSource(memStream.AsRandomAccessStream());
        handler.PlatformView.Source = bitmap;
    }
}