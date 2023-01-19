using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Text;
using ListenTogether.Model;
using ListenTogether.Network.Models.MiGu;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ListenTogether.Network.Utils;
public class NetEaseUtils
{
    private static readonly Random MyRandom = new Random();
    private const string RandomBaseString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const string SearchAESKey = "0CoJUm6Qyw8W8jud";
    private const string SearchAESIv = "0102030405060708";
    private const string PublicKey = "010001";
    private const string SearchModulus =
        "00e0b509f6259df8642dbc35662901477df22677ec152b5ff68ace615bb7b725152b3ab17a876aea8a5aa76d2e417629ec4ee341f56135fccf695280104e0312ecbda92557c93870114af6c9d05c4f7f0c3685b7a46bee255932575cce10b424d813cfe4875d3e82047b97ddef52741d546b8e289dc6935b3ece0462db0a22b8e7";

    public static IEnumerable<KeyValuePair<string, string>> GetPostDataForSuggest(string keyword)
    {
        string requestString = GetSuggestRequest(keyword);
        string num = GetRandom();
        string encText = CalcAES(requestString, SearchAESKey);
        encText = CalcAES(encText, num);
        string encSecKey = GetEncSecKey(num);
        var data = new Dictionary<string, string> { { "params", encText }, { "encSecKey", encSecKey } };
        return data;
    }

    private static string GetSuggestRequest(string keyword)
    {
        return "{\"s\":\"" + keyword + "\",\"limit\":\"8\",\"csrf_token\":\"\"}";
    }

    public static IEnumerable<KeyValuePair<string, string>> GetPostDataForSearch(string keyword)
    {
        string searchString = GetQueryString(keyword);
        string num = GetRandom();
        string encText = CalcAES(searchString, SearchAESKey);
        encText = CalcAES(encText, num);
        string encSecKey = GetEncSecKey(num);
        var data = new Dictionary<string, string> { { "params", encText }, { "encSecKey", encSecKey } };
        return data;
    }

    private static string GetQueryString(string keyword)
    {
        return
            "{\"hlpretag\":\"<span class=\\\"s-fc7\\\">\",\"hlposttag\":\"</span>\",\"s\":\"" + keyword + "\",\"type\":\"1\",\"offset\":\"0\",\"total\":\"true\",\"limit\":\"30\",\"csrf_token\":\"\"}";
    }
    private static string GetRandom()
    {
        string result = "";
        int length = 16;

        for (int i = 0; i < length; i++)
        {
            result = $"{result}{RandomBaseString[MyRandom.Next(0, RandomBaseString.Length)]}";
        }
        return result;
    }

    private static string CalcAES(string text, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var iv = Encoding.UTF8.GetBytes(SearchAESIv);
        return AESHelper.EncryptStringToBytes(text, keyBytes, iv);
    }
    private static string GetEncSecKey(string text)
    {
        //encSecKey这个参数实在是不想分析了，因此网上找了下别人写好的方法直接用了
        //地址:https://github.com/GEEKiDoS/NeteaseMuiscApi/blob/90e85514b5afe574ea5f54f93e4863d47e6f3b3d/NeteaseCloudMuiscApi.cs
        string tmpText = new string(text.Reverse().ToArray());
        var a = BCHexDec(BitConverter.ToString(Encoding.Default.GetBytes(tmpText)).Replace("-", ""));
        var b = BCHexDec(PublicKey);
        var c = BCHexDec(SearchModulus);
        string key = BigInteger.ModPow(a, b, c).ToString("x");
        key = key.PadLeft(256, '0');
        if (key.Length > 256)
            return key.Substring(key.Length - 256, 256);
        else
            return key;
    }

    private static BigInteger BCHexDec(string hex)
    {
        BigInteger dec = new BigInteger(0);
        int len = hex.Length;
        for (int i = 0; i < len; i++)
        {
            dec += BigInteger.Multiply(new BigInteger(Convert.ToInt32(hex[i].ToString(), 16)), BigInteger.Pow(new BigInteger(16), len - i - 1));
        }
        return dec;
    }

    public static IEnumerable<KeyValuePair<string, string>> GetPostDataForMusicUrl(string musicId)
    {
        string searchString = GetUrlString(musicId);
        string num = GetRandom();
        string encText = CalcAES(searchString, SearchAESKey);
        encText = CalcAES(encText, num);
        string encSecKey = GetEncSecKey(num);
        var data = new Dictionary<string, string> { { "params", encText }, { "encSecKey", encSecKey } };
        return data;
    }

    private static string GetUrlString(string musicId)
    {
        return "{\"ids\":\"[" + musicId + "]\",\"level\":\"standard\",\"encodeType\":\"aac\",\"csrf_token\":\"\"}";
    }

    public static IEnumerable<KeyValuePair<string, string>> GetPostDataForLyric(string musicId)
    {
        string searchString = GetLyricRequest(musicId);
        string num = GetRandom();
        string encText = CalcAES(searchString, SearchAESKey);
        encText = CalcAES(encText, num);
        string encSecKey = GetEncSecKey(num);
        var data = new Dictionary<string, string> { { "params", encText }, { "encSecKey", encSecKey } };
        return data;
    }

    private static string GetLyricRequest(string musicId)
    {
        return "{\"id\":" + musicId + ",\"lv\":-1,\"tv\":-1,\"csrf_token\":\"\"}";
    }

    public static List<MusicTag> GetHotTags(string html)
    {
        html = html.Replace("&amp;", "&");
        List<MusicTag> hotTags = new List<MusicTag>();

        string hotPattern = """
                            "/discover/playlist/\?cat=(?<Id>.+?)".+?>(?<Name>.+?)<
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
        return hotTags;
    }

    public static List<MusicTypeTag> GetAllTypes(string html)
    {
        html = html.Replace("&amp;", "&");
        List<MusicTypeTag> allTypes = new List<MusicTypeTag>();

        string pattern = """
                            <dl class="f-cb">[\s\S]*?</i>(?<TypeName>.+?)</dt>(?<TypeList>[\s\S]*?)</dl>
                            """;

        MatchCollection mc = Regex.Matches(html, pattern);
        for (int i = 0; i < mc.Count; i++)
        {
            string typeName = mc[i].Groups["TypeName"].Value;
            string typeList = mc[i].Groups["TypeList"].Value;

            string typePattern = """
                            "/discover/playlist/\?cat=(?<Id>[\s\S]*?)"[\s\S]*?>(?<Name>.+?)<
                            """;
            MatchCollection mcType = Regex.Matches(typeList, typePattern);

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
        return allTypes;
    }

    public static List<SongMenu> GetSongMenusFromTop()
    {
        var songMenus = new List<SongMenu>()
        {
             new()
             {
                 Id="19723756",
                 Name="飙升榜",
                 ImageUrl="http://p2.music.126.net/pcYHpMkdC69VVvWiynNklA==/109951166952713766.jpg?param=150y150",
                 LinkUrl="https://music.163.com//discover/toplist?id=19723756"
             },
             new()
             {
                 Id="3779629",
                 Name="新歌榜",
                 ImageUrl="http://p2.music.126.net/wVmyNS6b_0Nn-y6AX8UbpQ==/109951166952686384.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=3779629"
             },
             new()
             {
                 Id="2884035",
                 Name="原创榜",
                 ImageUrl="http://p2.music.126.net/iFZ_nw2V86IFk90dc50kdQ==/109951166961388699.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=2884035"
             },
             new()
             {
                 Id="3778678",
                 Name="热歌榜",
                 ImageUrl="http://p2.music.126.net/ZyUjc7K_GDpD8MO1-GQkmA==/109951166952706664.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=3778678"
             },
             new()
             {
                 Id="5453912201",
                 Name="黑胶VIP爱听榜",
                 ImageUrl="http://p2.music.126.net/-4Dy9H4RQyN7sS7OQplC8g==/109951168129993341.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=5453912201"
             },
             new()
             {
                 Id="7785123708",
                 Name="黑胶VIP新歌榜",
                 ImageUrl="http://p2.music.126.net/z1GrCMAt8FT-UDQh6Cjsug==/109951168129988440.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=7785123708"
             },
             new()
             {
                 Id="7785066739",
                 Name="黑胶VIP热歌榜",
                 ImageUrl="http://p2.music.126.net/4UWJ-6pdnIrnMxsXsXSuWQ==/109951168129994225.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=7785066739"
             },
             new()
             {
                 Id="7785091694",
                 Name="黑胶VIP爱搜榜",
                 ImageUrl="http://p2.music.126.net/ADzz0_8ZiSkbwFDI8Dw0jg==/109951168129999516.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=7785091694"
             },
             new()
             {
                 Id="991319590",
                 Name="云音乐说唱榜",
                 ImageUrl="http://p2.music.126.net/xNnQzUODQs50SJ2Sm4IVVA==/109951167976981051.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=991319590"
             },
             new()
             {
                 Id="71384707",
                 Name="云音乐古典榜",
                 ImageUrl="http://p2.music.126.net/urByD_AmfBDBrs7fA9-O8A==/109951167976973225.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=71384707"
             },
             new()
             {
                 Id="1978921795",
                 Name="云音乐电音榜",
                 ImageUrl="http://p2.music.126.net/lH6L0YhKTofSmnrEAtN9CA==/109951168204710928.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=1978921795"
             },
             new()
             {
                 Id="5059633707",
                 Name="云音乐摇滚榜",
                 ImageUrl="http://p2.music.126.net/UsoWOvtgwBgrofCCfS61Fw==/109951167976981586.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=5059633707"
             },
             new()
             {
                 Id="6723173524",
                 Name="网络热歌榜",
                 ImageUrl="http://p2.music.126.net/iwhTcAbujlsvhSNWYkBC8Q==/109951167430851785.jpg?param=150y150",
                 LinkUrl="https://music.163.com/discover/toplist?id=6723173524"
             }
        };

        return songMenus;
    }

    public static List<SongMenu> GetSongMenusFromTag(string html)
    {
        var songMenus = new List<SongMenu>();
        string songMenusPattern = """
                                <ul class="m-cvrlst f-cb"[\s\S]*?</ul>
                                """;

        MatchCollection mc = Regex.Matches(html, songMenusPattern);
        if (mc.Count != 1)
        {
            return songMenus;
        }

        string songMenuListPattern = """
                                <li>[\s\S]*?</li>
                                """;

        var songMenuHtmlList = RegexUtils.GetAll(mc[0].Value, songMenuListPattern);
        foreach (var songMenuHtml in songMenuHtmlList)
        {
            try
            {
                string songMenuPattern = """
                                <img[\s\S]*?src="(?<ImageUrl>[\s\S]*?)"[\s\S]*?title="(?<Name>[\s\S]*?)"[\s\S]*?href="(?<Href>[\s\S]*?)"
                                """;

                var (success, songMenuResult) = RegexUtils.GetMultiGroupInFirstMatch(songMenuHtml, songMenuPattern);
                if (!success)
                {
                    continue;
                }
                var href = songMenuResult["Href"] ?? "";
                if (href.IndexOf("?id=") == -1)
                {
                    continue;
                }
                var id = href.Substring(href.IndexOf("?id=") + "?id=".Length);

                var songMenu = new SongMenu()
                {
                    Id = id,
                    Name = songMenuResult["Name"] ?? "",
                    ImageUrl = songMenuResult["ImageUrl"] ?? "",
                    LinkUrl = $"https://music.163.com{href}"
                };

                songMenus.Add(songMenu);
            }
            catch (Exception)
            {
                continue;
            }
        }
        return songMenus;
    }

    public static IEnumerable<KeyValuePair<string, string>> GetPostDataForTagMusics(string id)
    {
        string requestString = GetTagMusicsRequest(id);
        string num = GetRandom();
        string encText = CalcAES(requestString, SearchAESKey);
        encText = CalcAES(encText, num);
        string encSecKey = GetEncSecKey(num);
        var data = new Dictionary<string, string> { { "params", encText }, { "encSecKey", encSecKey } };
        return data;
    }

    private static string GetTagMusicsRequest(string id)
    {
        var data = new { id, offset = 0, total = true, limit = 50, n = 50, csrf_token = "" };
        return data.ToJson();
    }
    public static List<HttpMusicTagResult> GetTagMusics(string html)
    {
        var musics = new List<HttpMusicTagResult>();
        string listDataPattern = """
                                class="J-btn-share"[\s\S]+?data-share='(?<Data>[\s\S]+?)'
                                """;
        var songs = RegexUtils.GetOneGroupAllMatch(html, listDataPattern);
        foreach (var song in songs)
        {
            try
            {
                var music = song.ToObject<HttpMusicTagResult>();
                if (music == null)
                {
                    continue;
                }
                musics.Add(music);
            }
            catch (Exception)
            {
            }
        }
        return musics;
    }
}

public class AESHelper
{
    public static string EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
    {
        // Check arguments.  
        if (plainText == null || plainText.Length <= 0)
        {
            throw new ArgumentNullException("plainText");
        }
        if (key == null || key.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }
        if (iv == null || iv.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }
        byte[] encrypted;
        // Create a RijndaelManaged object  
        // with the specified key and IV.  
        using (var rijAlg = Aes.Create())
        {
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Padding = PaddingMode.PKCS7;
            rijAlg.FeedbackSize = 128;

            rijAlg.Key = key;
            rijAlg.IV = iv;

            // Create a decrytor to perform the stream transform.  
            var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            // Create the streams used for encryption.  
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.  
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        // Return the encrypted bytes from the memory stream.  
        return Convert.ToBase64String(encrypted);
    }
}