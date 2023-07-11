using SQLite;

namespace ListenTogether.Data.Maui.Entities;

[Table("MusicCache")]
internal class MusicCacheEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [NotNull]
    public string MusicId { get; set; } = null!;
    [NotNull]
    public string FileName { get; set; } = null!;
    [NotNull]
    public string Remark { get; set; } = null!;
}
