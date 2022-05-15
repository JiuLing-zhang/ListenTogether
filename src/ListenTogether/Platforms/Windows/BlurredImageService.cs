namespace ListenTogether.Platforms.Windows;

public class BlurredImageService : IBlurredImageService
{
    public byte[] Convert(byte[] buffer)
    {
        var gaussianBlur = new GaussianBlur(buffer);
        return gaussianBlur.Process(20);
    }
}