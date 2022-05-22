using Microsoft.Maui.Handlers;

namespace ListenTogether.Handlers.GaussianBlurHandler;

internal partial class GaussianImageHandler
{
    public static PropertyMapper<IGaussianImage, GaussianImageHandler> Mapper = new(ViewHandler.ViewMapper)
    {
        [nameof(IGaussianImage.Url)] = MapUrl

    };

    public GaussianImageHandler(PropertyMapper mapper)
        : base(mapper)
    {

    }

    public GaussianImageHandler() : base(Mapper)
    {

    }
}