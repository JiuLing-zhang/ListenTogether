using SQLite;

namespace ListenTogether.Data.Entities;

[Table("MusicCache")]
internal class MusicCacheEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [NotNull]
    public string MusicId { get; set; } = null!;
    [NotNull]
    public string FileName { get; set; } = null!;
}
