using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerOnline.Api.Entities
{
    [Table("Playlist")]
    public class PlaylistEntity
    {
        public int UserBaseId { get; set; }
        public string MusicId { get; set; } = null!;
        public string MusicName { get; set; } = null!;
        public string MusicArtist { get; set; } = null!;
    }
}
