using ListenTogether.Model.Enums;
using SQLite;

namespace ListenTogether.Data.Entities;

[Table("Playlist")]
internal class PlaylistEntity
{
    [PrimaryKey]
    public string MusicId { get; set; } = null!;
    public PlatformEnum Platform { get; set; }
    public string MusicIdOnPlatform { get; set; } = null!;
    public string MusicName { get; set; } = null!;
    public string MusicArtist { get; set; } = null!;
    public string MusicAlbum { get; set; } = null!;
    public string MusicImageUrl { get; set; } = null!;
    public DateTime EditTime { get; set; }
}