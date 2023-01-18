using SQLite;

namespace ListenTogether.Data.Entities;

[Table("Music")]
internal class MusicEntity
{
    [PrimaryKey]
    public string Id { get; set; } = null!;
    public int Platform { get; set; }
    public string PlatformInnerId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Album { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string ExtendData { get; set; } = null!;
}