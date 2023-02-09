using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Model;
using ListenTogether.Network.Models.KuWo;
using System.Text.RegularExpressions;

namespace ListenTogether.Network.Utils;
internal class KuWoUtils
{
    public static List<string> GetHotWordFromHtml(string input)
    {
        string pattern = @"class=""songName wordType""[\s\S]*?>(?<Word>[\s\S]*?)<\/div>";
        var result = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupAllMatch(input, pattern);
        return result.Select(x => x.Trim()).ToList();
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
             }
        };

        return songMenus;
    }
}