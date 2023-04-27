namespace ListenTogether.Data;
public interface ISearchHistoryStorage
{
    int ListMaxCount { get; set; }
    void Add(string key);
    List<string> GetHistories();
    void Remove(string key);
    void Clear();
}