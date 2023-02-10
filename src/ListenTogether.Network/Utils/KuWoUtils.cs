using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Security;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Network.Models.KuWo;
using System.Text.RegularExpressions;

namespace ListenTogether.Network.Utils;
internal class KuWoUtils
{
    public static List<string> GetHotWord(string json)
    {
        var hotWords = new List<string>();
        var httpObj = json.ToObject<HttpHotWordResult>();
        if (httpObj == null || httpObj.code != 200 || httpObj.data == null)
        {
            return hotWords;
        }
        return httpObj.data;
    }

    public static List<MusicTag> GetHotTags(string html)
    {
        html = html.Replace("&amp;", "&");
        List<MusicTag> hotTags = new List<MusicTag>();

        string hotPattern = """
                            playlistTag[\s\S]*?\[(?<Tags>[\s\S]*?)\]
                            """;

        var (success, tagsHtml) = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, hotPattern);
        if (!success)
        {
            return hotTags;
        }

        string itemPattern = """
                            name:"\S+?"[\s\S]*?id:"\S+?"
                            """;
        MatchCollection mc = Regex.Matches(tagsHtml, itemPattern);
        if (mc.Count > 0)
        {
            for (int i = 0; i < mc.Count; i++)
            {
                string pattern = """
                    name:"(?<Name>\S+?)"[\s\S]*?id:"(?<Id>\d+?)"
                    """;
                var (tagsSuccess, tags) = JiuLing.CommonLibs.Text.RegexUtils.GetMultiGroupInFirstMatch(mc[i].Value, pattern);
                if (!tagsSuccess)
                {
                    continue;
                }
                hotTags.Add(new MusicTag()
                {
                    Id = tags["Id"],
                    Name = tags["Name"]
                });
            }
        }
        return hotTags;
    }

    public static List<MusicTypeTag> GetAllTypes(string json)
    {
        List<MusicTypeTag> allTypes = new List<MusicTypeTag>();
        var httpObj = json.ToObject<HttpMusicTagResult>();
        if (httpObj == null || httpObj.code != 200 || httpObj.data == null)
        {
            return allTypes;
        }
        foreach (var typeTag in httpObj.data)
        {
            var tags = new List<MusicTag>();
            foreach (var tag in typeTag.data)
            {
                tags.Add(new MusicTag()
                {
                    Id = tag.id,
                    Name = tag.name
                });
            }

            allTypes.Add(new MusicTypeTag()
            {
                TypeName = typeTag.name,
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
                 Id="93",
                 Name="酷我飙升榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/2/0/1675941807.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="17",
                 Name="酷我新歌榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/0/0/1675941789.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="16",
                 Name="酷我热歌榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/5/5/1675990419.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="158",
                 Name="抖音歌曲榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/4/3/1675991042.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="242",
                 Name="极品电音榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/8/5/1675990941.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="284",
                 Name="酷我热评榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/1/2/1675990946.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="187",
                 Name="流行趋势榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/6/9/1675990862.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="153",
                 Name="网红新歌榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/2/0/1675990889.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="26",
                 Name="经典怀旧榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/8/4/1675990896.png",
                 LinkUrl=""
             },
             new()
             {
                 Id="329",
                 Name="酷我说唱榜",
                 ImageUrl="https://img3.kuwo.cn/star/upload/1/5/1675990361.png",
                 LinkUrl=""
             }
        };

        return songMenus;
    }

    public static List<HttpTagSongMenuResultDataDatum> GetTagSongMenus(string json)
    {
        List<HttpTagSongMenuResultDataDatum> result = new List<HttpTagSongMenuResultDataDatum>();
        try
        {
            var httpObj = json.ToObject<HttpTagSongMenuResult>();
            if (httpObj == null || httpObj.code != 200 || httpObj.data == null || httpObj.data.data == null)
            {
                return result;
            }
            return httpObj.data.data;
        }
        catch (Exception ex)
        {
            Logger.Error("酷我歌单解析失败。", ex);
        }
        return result;
    }

    public static List<HttpSongMenuDataList> GetSongMenuMusics(string json)
    {
        List<HttpSongMenuDataList> result = new List<HttpSongMenuDataList>();
        try
        {
            var httpObj = json.ToObject<HttpSongMenuResult>();
            if (httpObj == null || httpObj.code != 200 || httpObj.data == null || httpObj.data.musicList == null)
            {
                return result;
            }
            return httpObj.data.musicList;
        }
        catch (Exception ex)
        {
            Logger.Error("酷我歌单歌曲解析失败。", ex);
        }
        return result;
    }
}