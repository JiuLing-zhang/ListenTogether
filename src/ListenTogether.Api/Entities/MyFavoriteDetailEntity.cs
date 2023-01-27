using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ListenTogether.Api.Entities
{
    [Table("MyFavoriteDetail")]
    [Owned]
    public class MyFavoriteDetailEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MyFavoriteId { get; set; }
        [ForeignKey("MyFavoriteId")]
        public virtual MyFavoriteEntity MyFavorite { get; set; } = null!;

        public string MusicId { get; set; } = null!;
    }
}
