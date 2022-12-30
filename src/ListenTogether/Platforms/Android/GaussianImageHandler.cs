using Android.Content;
using Android.Graphics;
using Android.Renderscripts;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;

namespace ListenTogether.Handlers.GaussianImage;
/// <summary>
/// 图像高斯模糊处理程序
/// </summary>
public partial class GaussianImageHandler : ViewHandler<IGaussianImage, ImageView>
{
    private const float BITMAP_SCALE = 0.3f;
    private const float RESIZE_SCALE = 0.2f;
    protected override ImageView CreatePlatformView()
    {
        var imageView = new ImageView(Context);
        //图片适配方式直接使用FitXy，如果后期需要在修改为自定义属性
        imageView.SetScaleType(ImageView.ScaleType.FitXy);
        return imageView;
    }

    static async void MapSourceByteArray(GaussianImageHandler handler, IGaussianImage gaussianBlurImage)
    {
        if (gaussianBlurImage.SourceByteArray == null)
        {
            return;
        }

        ImageSource source = ImageSource.FromStream(() => new MemoryStream(gaussianBlurImage.SourceByteArray));
        Bitmap bitmap = await GetImageFromImageSource(handler.Context, source);
        handler.PlatformView.SetImageBitmap(bitmap);
    }

    private static async Task<Bitmap> GetImageFromImageSource(Context context, ImageSource imageSource)
    {
        IImageSourceHandler handler;

        if (imageSource is FileImageSource)
        {
            handler = new FileImageSourceHandler();
        }
        else if (imageSource is StreamImageSource)
        {
            handler = new StreamImagesourceHandler(); // sic  
        }
        else if (imageSource is UriImageSource)
        {
            handler = new ImageLoaderSourceHandler(); // sic  
        }
        else
        {
            throw new NotImplementedException();
        }

        var originalBitmap = await handler.LoadImageAsync(imageSource, context);

        // Blur it twice!  
        var blurredBitmap = await Task.Run(() => CreateBlurredImage(context, CreateResizedImage(context, originalBitmap), 25));

        return blurredBitmap;
    }

    private static Bitmap CreateBlurredImage(Context context, Bitmap originalBitmap, int radius)
    {
        // Create another bitmap that will hold the results of the filter.  
        Bitmap blurredBitmap;
        blurredBitmap = Bitmap.CreateBitmap(originalBitmap);

        // Create the Renderscript instance that will do the work.  
        RenderScript rs = RenderScript.Create(context);

        // Allocate memory for Renderscript to work with  
        Allocation input = Allocation.CreateFromBitmap(rs, originalBitmap, Allocation.MipmapControl.MipmapFull, AllocationUsage.Script);
        Allocation output = Allocation.CreateTyped(rs, input.Type);

        // Load up an instance of the specific script that we want to use.  
        ScriptIntrinsicBlur script = ScriptIntrinsicBlur.Create(rs, Android.Renderscripts.Element.U8_4(rs));
        script.SetInput(input);

        // Set the blur radius  
        script.SetRadius(radius);

        // Start Renderscript working.  
        script.ForEach(output);

        // Copy the output to the blurred bitmap  
        output.CopyTo(blurredBitmap);

        return blurredBitmap;
    }

    private static Bitmap CreateResizedImage(Context context, Bitmap originalBitmap)
    {
        int width = Convert.ToInt32(Math.Round(originalBitmap.Width * BITMAP_SCALE));
        int height = Convert.ToInt32(Math.Round(originalBitmap.Height * BITMAP_SCALE));

        // Create another bitmap that will hold the results of the filter.  
        Bitmap inputBitmap = Bitmap.CreateScaledBitmap(originalBitmap, width, height, false);
        Bitmap outputBitmap = Bitmap.CreateBitmap(inputBitmap);

        // Create the Renderscript instance that will do the work.  
        RenderScript rs = RenderScript.Create(context);


        Allocation tmpIn = Allocation.CreateFromBitmap(rs, inputBitmap);
        Allocation tmpOut = Allocation.CreateFromBitmap(rs, outputBitmap);

        // Allocate memory for Renderscript to work with  
        var t = Android.Renderscripts.Type.CreateXY(rs, tmpIn.Element, Convert.ToInt32(width * RESIZE_SCALE), Convert.ToInt32(height * RESIZE_SCALE));
        Allocation tmpScratch = Allocation.CreateTyped(rs, t);

        ScriptIntrinsicResize theIntrinsic = ScriptIntrinsicResize.Create(rs);

        // Resize the original img down.  
        theIntrinsic.SetInput(tmpIn);
        theIntrinsic.ForEach_bicubic(tmpScratch);

        // Resize smaller img up.  
        theIntrinsic.SetInput(tmpScratch);
        theIntrinsic.ForEach_bicubic(tmpOut);
        tmpOut.CopyTo(outputBitmap);

        return outputBitmap;
    }
}