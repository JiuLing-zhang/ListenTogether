using Microsoft.Maui.Handlers;

namespace ListenTogether.Handlers.GaussianImage;

/// <summary>
/// 图像高斯模糊处理程序
/// </summary>
public partial class GaussianImageHandler
{
    public static PropertyMapper<ListenTogether.Handlers.GaussianImage.IGaussianImage, GaussianImageHandler> Mapper = new(ViewHandler.ViewMapper)
    {
        [nameof(ListenTogether.Handlers.GaussianImage.IGaussianImage.SourceByteArray)] = MapSourceByteArray
    };

    public GaussianImageHandler(PropertyMapper mapper)
        : base(mapper)
    {

    }

    public GaussianImageHandler() : base(Mapper)
    {

    }
}