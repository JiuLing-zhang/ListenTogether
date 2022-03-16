using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MusicPlayerOnline.Api.Entities;

[Owned]
[Table("RefreshToken")]
public class RefreshTokenEntity
{
    public int UserBaseId { get; set; }
    [ForeignKey("UserBaseId")]
    public virtual UserEntity User { get; set; } = null!;

    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpireTime { get; set; }
    public DateTime CreateTime { get; set; }
    public string CreateIp { get; set; } = null!;
    public bool IsExpired => DateTime.UtcNow >= ExpireTime;
    public bool IsActive => !IsExpired;
}