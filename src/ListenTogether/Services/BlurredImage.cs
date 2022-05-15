namespace ListenTogether.Services;

public interface IBlurredImageService
{
    byte[] Convert(byte[] buffer);
}