using ListenTogether.Model.Enums;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace ListenTogether.Model;

public class Music : MusicBase
{
    /// <summary>
    /// 平台
    /// </summary>
    public PlatformEnum Platform { get; set; }

    /// <summary>
    /// 平台名称
    /// </summary>
    public string PlatformName { get; set; } = null!;
    /// <summary>
    /// 对应平台的ID
    /// </summary>
    public string PlatformInnerId { get; set; } = null!;
    /// <summary>
    /// 扩展数据
    /// </summary>
    public string ExtendData { get; set; } = null!;

    /// <summary>
    /// 缓存文件名
    /// </summary>
    public string CacheFileName => FilterPathKeyword($"{PlatformName}-{Name}-{Artist}.music");

    /// <summary>
    /// 缓存歌词文件名
    /// </summary>
    public string CacheLyricFileName => FilterPathKeyword($"{PlatformName}-{Name}-{Artist}.lrc");

    /// <summary>
    /// 过滤路径关键字
    /// </summary>
    private string FilterPathKeyword(string input)
    {
        string pattern = @"[\:\/\\\*\?\""\<\>\|]";
        return new Regex(pattern).Replace(input, "-");
    }
}