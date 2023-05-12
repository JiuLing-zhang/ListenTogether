using ListenTogether.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogetherMauiBlazor.Storages;
public class PlayHistoryStorage : IPlayHistoryStorage
{
    public Task<string> GetLastMusicIdAsync()
    {
        return Task.FromResult(Preferences.Get("LastMusicId", ""));
    }

    public Task SetLastMusicIdAsync(string id)
    {
        Preferences.Set("LastMusicId", id);
        return Task.CompletedTask;
    }
}