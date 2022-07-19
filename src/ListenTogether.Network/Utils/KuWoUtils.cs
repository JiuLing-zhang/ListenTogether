namespace ListenTogether.Network.Utils;
internal class KuWoUtils
{
    public static List<string> GetHotWordFromHtml(string input)
    {
        string pattern = @"class=""songName wordType""[\s\S]*?>(?<Word>[\s\S]*?)<\/div>";
        var result = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupAllMatch(input, pattern);
        return result.Select(x => x.Trim()).ToList();
    }
}