using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Storages;

namespace ListenTogether.ViewModels;

public partial class CacheCleanViewModel : ViewModelBase
{
    private readonly IMusicCacheStorage _musicCacheStorage;
    public CacheCleanViewModel(IMusicCacheStorage musicCacheStorage)
    {
        Caches = new ObservableCollectionEx<MusicFileViewModel>();
        _musicCacheStorage = musicCacheStorage;
    }
    public async Task InitializeAsync()
    {
        try
        {
            Loading("拼命计算中....");
            SelectedSize = 0;
            await QueryCachesAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("缓存文件查找失败");
            Logger.Error("缓存文件查找失败。", ex);
        }
        finally
        {
            LoadComplete();
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AllSizeString))]
    private Int64 _allSize;
    public string AllSizeString => SizeToString(AllSize);


    [ObservableProperty]
    private Int64 _selectedSize;

    [ObservableProperty]
    private ObservableCollectionEx<MusicFileViewModel> _caches = null!;

    private async Task QueryCachesAsync()
    {
        Caches.Clear();
        AllSize = 0;

        await _musicCacheStorage.CalcCacheSizeAsync((length) =>
        {
            var size = Convert.ToInt64(length);
            var musicFile = new MusicFileViewModel()
            {
                Id = 0,
                Remark = "歌曲名的占位符而已",
                FileName = "",
                Size = size
            };
            Caches.Add(musicFile);
            AllSize = AllSize + size;
        });

    }

    [RelayCommand]
    private async void ClearAsync()
    {
        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除吗？删除后不可恢复。", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            Loading("处理中....");
            await _musicCacheStorage.ClearCacheAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("删除失败");
            Logger.Error("缓存文件删除失败。", ex);
        }
        finally
        {
            LoadComplete();
        }

        await QueryCachesAsync();
        await ToastService.Show("删除完成");
    }

    private string SizeToString(Int64 size)
    {
        return $"{size / 1024 / 1024:N2}";
    }
}