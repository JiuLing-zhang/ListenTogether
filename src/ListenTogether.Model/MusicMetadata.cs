namespace ListenTogether.Model;
public class MusicMetadata
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public byte[] Image { get; set; }
    public string FilePath { get; set; }

    public MusicMetadata(string id, string name, string artist, string album, byte[] image, string filePath)
    {
        Id = id;
        Name = name;
        Artist = artist;
        Album = album;
        Image = image;
        FilePath = filePath;
    }
}