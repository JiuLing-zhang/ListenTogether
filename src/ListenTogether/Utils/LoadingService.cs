using CommunityToolkit.Maui.Views;
using ListenTogether.Controls;
using System.Collections.Concurrent;

namespace ListenTogether.Utils;
public class LoadingService
{
    private static ConcurrentDictionary<string, LoadingPage> LoadingPages = new ConcurrentDictionary<string, LoadingPage>();
    private LoadingService()
    {

    }

    public static void Loading(string key, string message)
    {
        Page page = Application.Current?.MainPage;
        if (page == null)
        {
            Logger.Info($"无法初始化Loading，message={message}");
            return;
        }

        var loadingPage = new LoadingPage(message);
        if (!LoadingPages.TryAdd(key, loadingPage))
        {
            return;
        }

        try
        {
            page.ShowPopup(loadingPage);
        }
        catch (Exception ex)
        {
            Logger.Error($"Loading失败,message={message}", ex);
            RemovePage(key);
        }
    }

    public static void LoadComplete(string key)
    {
        if (!LoadingPages.TryGetValue(key, out LoadingPage page))
        {
            return;
        }
        page.Close();
        RemovePage(key);
    }

    private static void RemovePage(string key)
    {
        var isRemove = LoadingPages.TryRemove(key, out _);
        if (!isRemove)
        {
            Logger.Info($"删除Loading页面失败，{key}");
        }
    }
}