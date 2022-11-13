using System.Text.Json;

namespace ListenTogether.Storage;
public class SearchHistoryStorage
{
    /// <summary>
    /// 历史记录列表的最大数量
    /// </summary>
    public static int ListMaxCount { get; set; } = 12;
    public static void Add(string key)
    {
        key = key.ToLower();
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        var histories = GetHistories();
        if (histories.Contains(key))
        {
            histories.Remove(key);
        }

        if (histories.Count == ListMaxCount)
        {
            histories.RemoveAt(ListMaxCount - 1);
        }

        histories.Insert(0, key);
        SetHistories(JsonSerializer.Serialize(histories));
    }

    public static IList<string> GetHistories()
    {
        string json = Preferences.Get("Histories", "");
        if (string.IsNullOrEmpty(json))
        {
            return new List<string>();
        }
        return JsonSerializer.Deserialize<List<string>>(json);
    }

    public static void Clear()
    {
        SetHistories(JsonSerializer.Serialize(new List<string>()));
    }

    private static void SetHistories(string histories)
    {
        Preferences.Set("Histories", histories);
    }
}