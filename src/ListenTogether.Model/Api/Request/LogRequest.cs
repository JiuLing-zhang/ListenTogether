using System.ComponentModel.DataAnnotations;

namespace ListenTogether.Model.Api.Request;
public class LogRequest
{
    public Int64 Timestamp { get; set; }
    public int LogType { get; set; }

    [Required]
    public string Message { get; set; } = null!;
}
