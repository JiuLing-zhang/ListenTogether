using System.ComponentModel.DataAnnotations;
using MusicPlayerOnline.Model.Enums;
namespace MusicPlayerOnline.Model.ApiRequest;
public class Log
{
    public Int64 Timestamp { get; set; }
    public LogTypeEnum LogType { get; set; }

    [Required]
    public string Message { get; set; } = null!;
}
