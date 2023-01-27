using ListenTogether.Model.Enums;
using SQLite;

namespace ListenTogether.Data.Entities;

[Table("Playlist")]
internal class PlaylistEntity
{
    [PrimaryKey]
    public string MusicId { get; set; } = null!;
    public PlatformEnum Platform { get; set; }
    public string IdOnPlatform { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Album { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int DurationMillisecond { get; set; }
    public string ExtendDataJson { get; set; } = null!;
    public DateTime EditTime { get; set; }
}