using ListenTogether.EasyLog;
using ListenTogether.Network.Models.MiGu;
using System.Web;
using JiuLing.CommonLibs.ExtensionMethods;

namespace ListenTogether.Network.Utils;
public class MiGuUtils
{
    private static string _channelId = "";
    private static string _sourceId = "";
    private static string _appVersion = "";
    public static string CookieId = "fa129e1a-1a78-46e2-836e-9f64e033ecf9-n41660969419455";

    public static void SetCommonArgs(string sourceId, string channelId, string appVersion)
    {
        _sourceId = sourceId;
        _channelId = channelId;
        _appVersion = appVersion;
    }

    public static string GetSearchArgs(string keyword)
    {
        var userAgent = JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeUserAgent;

        keyword = HttpUtility.UrlEncode(keyword).Replace("+", "%20").ToUpper();
        var c = _channelId;
        var f = "html";
        var k = CookieId;
        var s = JiuLing.CommonLibs.Text.TimestampUtils.GetLen10();
        var u = $"{userAgent}/{_sourceId}";
        var v = _appVersion;

        var signString = $"c{c}f{f}k{k}keyword{keyword}s{s}u{u}v{v}";
        signString = HttpUtility.UrlEncode(signString).Replace("+", "%20").Replace("%2f", "%2F").Replace("%2c", "%2C").Replace("%3b", "%3B");
        var sign = JiuLing.CommonLibs.Security.SHA1Utils.GetStringValueToLower(signString);
        return $"page=1&type=song&i={sign}&f={f}&s={s}&c={c}&keyword={keyword}&v={v}";
    }
    public static bool TryScanSearchResult(string html, out List<HttpMusicSearchResult> musics)
    {
        musics = new List<HttpMusicSearchResult>();

        string pattern = @"data-share='(?<MusicInfo>[\s\S]*?)'";
        var htmlMusics = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupAllMatch(html, pattern);
        if (htmlMusics == null || htmlMusics.Count == 0)
        {
            return false;
        }



        foreach (var htmlMusic in htmlMusics)
        {
            try
            {
                if (htmlMusic.IsEmpty())
                {
                    continue;
                }

                var musicObj = System.Text.Json.JsonSerializer.Deserialize<HttpMusicSearchResult>(htmlMusic);
                if (musicObj == null || musicObj.type != "song")
                {
                    continue;
                }
                musics.Add(musicObj);
            }
            catch (Exception ex)
            {
                Logger.Error("构建咪咕搜索结果失败。", ex);
            }
        }
        return true;
    }

    public static (bool success, string id, string type) GetMusicRealArgs(string url)
    {
        string pattern = @"id=(?<id>\d*)&type=(?<type>\d*)";
        var resultGroup = JiuLing.CommonLibs.Text.RegexUtils.GetMultiGroupInFirstMatch(url, pattern);

        if (resultGroup.success == false)
        {
            return (false, "", "");
        }
        return (true, resultGroup.result["id"], resultGroup.result["type"]);
    }

    public static string GetPlayUrlData(string id, string copyrightId)
    {
        var time = DateTime.Now;
        Int64 timestampLast = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToLen13(time.AddSeconds(-1));
        Int64 timestamp = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToLen13(time);
        return $"id={id}&copyrightId={copyrightId}&resourceType=2&_={timestampLast}&v={timestamp}";
    }

    public static string GetPlayUrlPath(List<NewRateFormats> playResources)
    {
        string ftpHeadPattern = @"^ftp://\d+\.\d+\.\d+\.\d+:\d+";
        foreach (var playInfo in playResources.Where(x => x.url.IsNotEmpty()).OrderByDescending(x => x.SizeInt))
        {
            if (!JiuLing.CommonLibs.Text.RegexUtils.IsMatch(playInfo.url, ftpHeadPattern))
            {
                continue;
            }
            return JiuLing.CommonLibs.Text.RegexUtils.Replace(playInfo.url, ftpHeadPattern, "");
        }
        return "";
    }
}