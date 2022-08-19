using ListenTogether.EasyLog;
using ListenTogether.Network.Models.MiGu;
using System.Web;

namespace ListenTogether.Network.Utils;
public class MiGuUtils
{
    public static string GetSearchArgs(string keyword)
    {
        keyword = HttpUtility.UrlEncode(keyword).Replace("+", "%20").ToUpper();
        var c = "001002A";
        var f = "html";
        var k = "52bf846c-c287-4200-b629-888f37402e71-n41660916240309";
        var s = JiuLing.CommonLibs.Text.TimestampUtils.GetLen10();
        var u = JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeUserAgent;
        var v = "3.23.5";
        keyword = "孤勇者";
        var signString = $"c{c}f{f}k{k}keyword{keyword}s{s}u{u}v{v}";
        signString = HttpUtility.UrlEncode(signString).Replace("+", "%20").Replace("%2f", "%2F").Replace("%2c", "%2C").Replace("%3b", "%3B");
        var sign = JiuLing.CommonLibs.Security.SHA1Utils.GetStringValueToLower(signString);
        return $"page=1&type=song&i={sign}&f={f}&s={s}&c={c}&keyword={keyword}&v={v}";
    }
    public static bool TryScanSearchResult(string html, out List<HttpMusicSearchResult> musics)
    {
        musics = new List<HttpMusicSearchResult>();

        string pattern = @"<ul class=""list\"">(?<data>[\S\s]*)search_load_next";
        var result = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, pattern);
        if (result.success == false)
        {
            return false;
        }

        pattern = @"<li class=""default"">[\S\s]*?</li>";
        var musicHtmlList = JiuLing.CommonLibs.Text.RegexUtils.GetAll(result.result, pattern);
        if (musicHtmlList.Count == 0)
        {
            return false;
        }

        foreach (var musicHtml in musicHtmlList)
        {
            try
            {
                pattern = @"<span class=""search_type"">【音乐】";
                if (!JiuLing.CommonLibs.Text.RegexUtils.IsMatch(musicHtml, pattern))
                {
                    continue;
                }
                pattern = @"a href=""(?<musicPageUrl>\S*)""[\s\S]*click_content_id:\s*'(?<id>\S+)'[\s\S]*<img src=""(?<imageUrl>\S*)""[\s\S]*class=""search_type"">(?<name>[\S\s]*)</h3>[\s\S]*class=""desc"">(?<artist>[\S\s]*)</p>";
                var resultGroup = JiuLing.CommonLibs.Text.RegexUtils.GetMultiGroupInFirstMatch(musicHtml, pattern);

                if (resultGroup.success == false)
                {
                    continue;
                }
                pattern = @"<\/?.+?\/?>";
                var regex = new System.Text.RegularExpressions.Regex(pattern);

                string id = resultGroup.result["id"];

                string musicPageUrl = resultGroup.result["musicPageUrl"];
                string imageUrl = resultGroup.result["imageUrl"];

                string name = resultGroup.result["name"];
                name = regex.Replace(name, "");
                name = name.Replace("【音乐】", "");
                name = name.Trim();

                string artist = resultGroup.result["artist"];
                artist = regex.Replace(artist, "");
                artist = artist.Replace("歌手：", "");
                artist = artist.Trim();

                musics.Add(new HttpMusicSearchResult()
                {
                    Id = id,
                    Name = name,
                    Artist = artist,
                    ImageUrl = imageUrl,
                    MusicPageUrl = musicPageUrl
                });
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
        foreach (var playInfo in playResources.OrderByDescending(x => x.SizeInt))
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