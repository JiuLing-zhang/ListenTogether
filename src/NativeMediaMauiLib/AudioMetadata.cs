namespace NativeMediaMauiLib;
public class AudioMetadata
{
    public byte[] Image { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }

    public AudioMetadata(byte[] image, string name, string artist, string album)
    {
        Image = image;
        Name = name;
        Artist = artist;
        Album = album;
    }
}