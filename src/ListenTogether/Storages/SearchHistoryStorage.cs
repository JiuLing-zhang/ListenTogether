using System.Text.Json;
using ListenTogether.Data.Api;

namespace ListenTogether.Storages;
public class SearchHistoryStorage : ISearchHistoryStorage
{
    /// <summary>
    /// 历史记录列表的最大数量
    /// </summary>
    public int ListMaxCount { get; set; } = 12;
    public void Add(string key)
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

    public List<string> GetHistories()
    {
        string json = Preferences.Get("Histories", "");
        if (string.IsNullOrEmpty(json))
        {
            return new List<string>();
        }
        return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
    }
    public void Remove(string key)
    {
        var histories = GetHistories();
        if (!histories.Contains(key))
        {
            return;
        }
        histories.Remove(key);
        SetHistories(JsonSerializer.Serialize(histories));
    }

    public void Clear()
    {
        SetHistories(JsonSerializer.Serialize(new List<string>()));
    }

    private void SetHistories(string histories)
    {
        Preferences.Set("Histories", histories);
    }


}