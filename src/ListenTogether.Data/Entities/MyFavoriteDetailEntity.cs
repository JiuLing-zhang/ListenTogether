using SQLite;

namespace ListenTogether.Data.Entities;

[Table("MyFavoriteDetail")]
internal class MyFavoriteDetailEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int MyFavoriteId { get; set; }
    public string PlatformName { get; set; } = null!;
    public string MusicId { get; set; } = null!;
    public string MusicName { get; set; } = null!;
    public string MusicArtist { get; set; } = null!;
    public string MusicAlbum { get; set; } = null!;
}