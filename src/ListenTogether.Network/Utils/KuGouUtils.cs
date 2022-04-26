using System.Security.Cryptography;
using System.Text;

namespace ListenTogether.Network.Utils;
public class KuGouUtils
{
    private const string SearchKey = "NVPh5oo715z5DIWAeQlhMDsWXXQV4hwt";
    public static string GetSearchData(string keyword)
    {
        Int64 timestamp = JiuLing.CommonLibs.Text.TimestampUtils.GetLen13();
        string bitrate = "0";
        string callback = "callback123";
        string clientver = "2000";
        string dfid = "-";
        string inputtype = "0";
        string iscorrection = "1";
        string isfuzzy = "0";
        string page = "1";
        string pagesize = "30";
        string platform = "WebFilter";
        string privilege_filter = "0";
        string srcappid = "2919";
        string tag = "em";
        string userid = "0";
        string md5Key =
            $"{SearchKey}bitrate={bitrate}callback={callback}clienttime={timestamp}clientver={clientver}dfid={dfid}inputtype={inputtype}iscorrection={iscorrection}isfuzzy={isfuzzy}keyword={keyword}mid={timestamp}page={page}pagesize={pagesize}platform={platform}privilege_filter={privilege_filter}srcappid={srcappid}tag={tag}userid={userid}uuid={timestamp}{SearchKey}";
        string md5 = StringMd5(md5Key).ToUpper();
        return $"callback={callback}&keyword={keyword}&page={page}&pagesize={pagesize}&bitrate={bitrate}&isfuzzy={isfuzzy}&tag={tag}&inputtype={inputtype}&platform={platform}&userid={userid}&clientver={clientver}&iscorrection={iscorrection}&privilege_filter={privilege_filter}&srcappid={srcappid}&clienttime={timestamp}&mid={timestamp}&uuid={timestamp}&dfid={dfid}&signature={md5}";
    }

    private static string StringMd5(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] buffer = Encoding.Default.GetBytes(input);
        byte[] md5Buffer = md5.ComputeHash(buffer);
        string str = null;
        foreach (byte b in md5Buffer)
        {
            str += b.ToString("x2");
        }

        return str;
    }

    public static string RemoveHttpResultHead(string input)
    {
        string pattern = @"callback123\((?<data>[\s\S]*)\)";
        var result = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(input, pattern);
        if (result.success)
        {
            return result.result;
        }

        return "";
    }

    public static string RemoveSongNameTag(string input)
    {
        string pattern = @"<\/?.+?\/?>";
        var regex = new System.Text.RegularExpressions.Regex(pattern);
        return regex.Replace(input, "");

    }

    public static string GetMusicUrlData(string hash, string albumId)
    {
        var mid = StringMd5(new Guid().ToString("d"));
        Int64 timestamp = JiuLing.CommonLibs.Text.TimestampUtils.GetLen13();
        return $"r=play/getdata&hash={hash}&mid={mid}&album_id={albumId}&_={timestamp}";
    }

    public static string GetMusicLyricData(string hash, string albumId)
    {
        var mid = StringMd5(new Guid().ToString("d"));
        Int64 timestamp = JiuLing.CommonLibs.Text.TimestampUtils.GetLen13();
        return $"r=play/getdata&hash={hash}&mid={mid}&album_id={albumId}&_={timestamp}";
    }
}