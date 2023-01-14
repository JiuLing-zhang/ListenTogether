using ListenTogether.EasyLog;
using ListenTogether.Network.Models.MiGu;
using System.Web;
using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Model.Enums;
using ListenTogether.Model;
using JiuLing.CommonLibs.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public static string GetPlayUrlPath(List<NewRateFormats> playResources, MusicFormatTypeEnum musicFormatType)
    {
        switch (musicFormatType)
        {
            case MusicFormatTypeEnum.PQ:
                return GetPQPlayUrlPath(playResources);
            case MusicFormatTypeEnum.HQ:
                return GetHQPlayUrlPath(playResources);
            case MusicFormatTypeEnum.SQ:
                return GetSQPlayUrlPath(playResources);
            case MusicFormatTypeEnum.ZQ:
                return GetZQPlayUrlPath(playResources);
            default:
                throw new ArgumentOutOfRangeException(nameof(musicFormatType), musicFormatType, "不支持的音质配置");
        }
    }

    private static readonly string FtpHeadPattern = @"^ftp://\d+\.\d+\.\d+\.\d+:\d+";
    private static string GetZQPlayUrlPath(List<NewRateFormats> playResources)
    {
        var zqResource = playResources.FirstOrDefault(x => x.formatType == "ZQ");
        if (zqResource != null && JiuLing.CommonLibs.Text.RegexUtils.IsMatch(zqResource.iosUrl, FtpHeadPattern))
        {
            return JiuLing.CommonLibs.Text.RegexUtils.Replace(zqResource.iosUrl, FtpHeadPattern, "");
        }
        return GetSQPlayUrlPath(playResources);
    }

    private static string GetSQPlayUrlPath(List<NewRateFormats> playResources)
    {
        var sqResource = playResources.FirstOrDefault(x => x.formatType == "SQ");
        if (sqResource != null && JiuLing.CommonLibs.Text.RegexUtils.IsMatch(sqResource.iosUrl, FtpHeadPattern))
        {
            return JiuLing.CommonLibs.Text.RegexUtils.Replace(sqResource.iosUrl, FtpHeadPattern, "");
        }
        return GetHQPlayUrlPath(playResources);
    }

    private static string GetHQPlayUrlPath(List<NewRateFormats> playResources)
    {
        var hqResource = playResources.FirstOrDefault(x => x.formatType == "HQ");
        if (hqResource != null && JiuLing.CommonLibs.Text.RegexUtils.IsMatch(hqResource.url, FtpHeadPattern))
        {
            return JiuLing.CommonLibs.Text.RegexUtils.Replace(hqResource.url, FtpHeadPattern, "");
        }

        return GetPQPlayUrlPath(playResources);
    }

    private static string GetPQPlayUrlPath(List<NewRateFormats> playResources)
    {
        var pqResource = playResources.FirstOrDefault(x => x.formatType == "PQ");
        if (pqResource != null && JiuLing.CommonLibs.Text.RegexUtils.IsMatch(pqResource.url, FtpHeadPattern))
        {
            return JiuLing.CommonLibs.Text.RegexUtils.Replace(pqResource.url, FtpHeadPattern, "");
        }
        return "";
    }

    public static (List<MusicTag> HotTags, List<MusicTypeTag> AllTypes) GetTags(string html)
    {
        List<MusicTag>? hotTags = new List<MusicTag>();
        List<MusicTypeTag> allTypes = new List<MusicTypeTag>();

        string hotPattern = """
                            class=\"hottag\"><a\shref=\"(\S+)tagId=(?<Id>\d+)">(?<Name>\S+?)<
                            """;

        MatchCollection mc = Regex.Matches(html, hotPattern);
        if (mc.Count > 0)
        {
            for (int i = 0; i < mc.Count; i++)
            {
                hotTags.Add(new MusicTag()
                {
                    Id = mc[i].Groups["Id"].Value,
                    Name = mc[i].Groups["Name"].Value
                });
            }
        }

        string allPattern = """
                            class="tag-name">(?<TypeName>\S+)</div>[\s\S]*?class="tag-list"(?<TypeList>[\s\S]*?)ul>
                            """;
        mc = Regex.Matches(html, allPattern);
        if (mc.Count > 0)
        {
            for (int i = 0; i < mc.Count; i++)
            {
                string typeName = mc[i].Groups["TypeName"].Value;
                string typeList = mc[i].Groups["TypeList"].Value;

                string typePattern = """
                            <a\shref=\"(\S+)tagId=(?<Id>\d+)">(?<Name>\S+?)<
                            """;
                MatchCollection mcType = Regex.Matches(typeList, typePattern);

                if (mcType.Count == 0)
                {
                    continue;
                }

                var tags = new List<MusicTag>();
                for (int j = 0; j < mcType.Count; j++)
                {
                    tags.Add(new MusicTag()
                    {
                        Id = mcType[j].Groups["Id"].Value,
                        Name = mcType[j].Groups["Name"].Value
                    });
                }

                allTypes.Add(new MusicTypeTag()
                {
                    TypeName = typeName,
                    Tags = tags
                });
            }
        }
        return (hotTags, allTypes);
    }

    public static List<SongMenu> GetSongMenusFromTag(string html)
    {
        var songMenus = new List<SongMenu>();
        string songMenusPattern = """
                                class="song-list-all container"[\s\S]+class="page"
                                """;

        MatchCollection mc = Regex.Matches(html, songMenusPattern);
        if (mc.Count != 1)
        {
            return songMenus;
        }

        string songMenuPattern = """
                                data-share='(?<Data>[\s\S]+?)'>
                                """;

        var songMenuJson = RegexUtils.GetOneGroupAllMatch(mc[0].Value, songMenuPattern);
        foreach (var json in songMenuJson)
        {
            try
            {
                var obj = json.ToObject<HttpSongMenuResult>();
                if (obj == null)
                {
                    continue;
                }
                songMenus.Add(new SongMenu()
                {
                    ImageUrl = $"https:{obj.imgUrl}",
                    LinkUrl = obj.linkUrl,
                    Name = obj.title
                });
            }
            catch (Exception)
            {
                continue;
            }

        }
        return songMenus;
    }
}