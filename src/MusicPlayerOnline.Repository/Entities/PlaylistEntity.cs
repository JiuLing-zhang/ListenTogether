using SQLite;

namespace MusicPlayerOnline.Repository.Entities
{
    [Table("Playlist")]
    public class PlaylistEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string MusicId { get; set; } = null!;
        public string MusicName { get; set; } = null!;
        public string MusicArtist { get; set; } = null!;
    }
}
