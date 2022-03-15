using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MusicPlayerOnline.Api.Entities;

[Owned]
public class RefreshTokenEntity
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = null!;
    public string ReplacedByToken { get; set; } = null!;
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => !IsExpired;
}