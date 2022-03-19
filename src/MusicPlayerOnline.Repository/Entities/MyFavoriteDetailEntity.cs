using SQLite;

namespace MusicPlayerOnline.Repository.Entities
{
    [Table("MyFavoriteDetail")]
    public class MyFavoriteDetailEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MyFavoriteId { get; set; }
        public int Platform { get; set; }
        public string MusicId { get; set; } = null!;
        public string MusicName { get; set; } = null!;
        public string MusicArtist { get; set; } = null!;
        public string MusicAlbum { get; set; } = null!;
    }
}
