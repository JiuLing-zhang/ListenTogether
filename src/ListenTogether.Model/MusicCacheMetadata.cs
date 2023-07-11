namespace ListenTogether.Model;
public class MusicCacheMetadata
{
    public string FileExtension { get; set; }
    public byte[] Buffer { get; set; }
    public MusicCacheMetadata(string fileExtension, byte[] buffer)
    {
        FileExtension = fileExtension;
        Buffer = buffer;
    }
}