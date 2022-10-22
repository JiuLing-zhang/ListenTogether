using System.Text.Json;

namespace ListenTogether.Storage;
public class SearchHistoryStorage
{
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
            return;
        }

        histories.Add(key);
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