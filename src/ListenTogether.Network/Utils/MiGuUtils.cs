using ListenTogether.EasyLog;
using ListenTogether.Network.Models.MiGu;

namespace ListenTogether.Network.Utils;
public class MiGuUtils
{
    public static string GetSearchData(string keyword)
    {
        return $"migu_p=h5&pn=1&type=allLobby&_ch=&content={keyword}";
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
}