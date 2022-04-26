using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerOnline.Api.Entities
{
    [Table("Playlist")]
    public class PlaylistEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserBaseId { get; set; }
        public string PlatformName { get; set; } = null!;
        public string MusicId { get; set; } = null!;
        public string MusicName { get; set; } = null!;
        public string MusicArtist { get; set; } = null!;
        public string MusicAlbum { get; set; } = null!;
    }
}
