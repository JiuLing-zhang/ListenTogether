using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListenTogether.Api.Entities
{
    [Table("Music")]
    public class MusicEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InnerId { get; set; }
        public string Id { get; set; } = null!;
        public int Platform { get; set; }
        public string IdOnPlatform { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Artist { get; set; } = null!;
        public string Album { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string ExtendData { get; set; } = null!;
        public DateTime CreateTime { get; set; }
    }
}
