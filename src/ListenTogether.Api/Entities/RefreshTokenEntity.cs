using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ListenTogether.Api.Entities;

[Owned]
[Table("RefreshToken")]
public class RefreshTokenEntity
{
    public int UserBaseId { get; set; }
    [ForeignKey("UserBaseId")]
    public virtual UserEntity User { get; set; } = null!;

    [Key]
    [JsonIgnore]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string DeviceId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime ExpireTime { get; set; }
    public DateTime CreateTime { get; set; }
    public bool IsExpired => DateTime.Now >= ExpireTime;
}