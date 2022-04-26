using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerOnline.Api.Entities;

[Table("LogDetail")]
public class LogEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public int UserBaseId { get; set; }
    public string LogType { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime LogTime { get; set; }
    public DateTime CreateTime { get; set; }
}