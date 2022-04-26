using SQLite;

namespace MusicPlayerOnline.Data.Entities;

[Table("MyFavorite")]
internal class MyFavoriteEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int MusicCount { get; set; }
}