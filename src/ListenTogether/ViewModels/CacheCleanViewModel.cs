using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ListenTogether.ViewModels;

public partial class CacheCleanViewModel : ViewModelBase
{
    private readonly IMusicCacheService _musicCacheService;
    public CacheCleanViewModel(IMusicCacheService musicCacheService)
    {
        Caches = new ObservableCollectionEx<MusicFileViewModel>();
        Caches.ItemChanged += Caches_ItemChanged;
        Caches.CollectionChanged += Caches_CollectionChanged;
        _musicCacheService = musicCacheService;
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
    [NotifyPropertyChangedFor(nameof(SelectedSizeString))]
    private Int64 _selectedSize;
    public string SelectedSizeString => SizeToString(SelectedSize);

    [ObservableProperty]
    private ObservableCollectionEx<MusicFileViewModel> _caches = null!;

    private void Caches_ItemChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        CalcSelectedSize();
    }
    private void Caches_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        CalcSelectedSize();
    }
    public void CalcSelectedSize()
    {
        SelectedSize = 0;
        foreach (var cache in Caches)
        {
            if (cache.IsChecked == true)
            {
                SelectedSize = SelectedSize + cache.Size;
            }
        }
    }

    private async Task QueryCachesAsync()
    {
        Caches.Clear();
        AllSize = 0;
        var caches = await _musicCacheService.GetAllAsync();
        foreach (var cache in caches)
        {
            long size = 0;
            if (File.Exists(cache.FileName))
            {
                size = new FileInfo(cache.FileName).Length;
            }
            var musicFile = new MusicFileViewModel()
            {
                Id = cache.Id,
                Remark = cache.Remark,
                FileName = cache.FileName,
                Size = size,
                IsChecked = false
            };
            Caches.Add(musicFile);
            AllSize = AllSize + size;
        }
    }

    [RelayCommand]
    private void SelectAll()
    {
        if (Caches.Count == Caches.Count(x => x.IsChecked == true))
        {
            foreach (var cache in Caches)
            {
                cache.IsChecked = false;
            }
        }
        else
        {
            foreach (var cache in Caches)
            {
                cache.IsChecked = true;
            }
        }
    }

    [RelayCommand]
    private void RowClickedAsync(MusicFileViewModel selected)
    {
        selected.IsChecked = !selected.IsChecked;
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
            foreach (var cache in Caches)
            {
                if (cache.IsChecked == true)
                {
                    try
                    {
                        if (File.Exists(cache.FileName))
                        {
                            System.IO.File.Delete(cache.FileName);
                        }
                        await _musicCacheService.RemoveAsync(cache.Id);
                    }
                    catch (Exception ex)
                    {
                        await ToastService.Show($"文件删除失败：{cache.FileName}");
                    }
                }
            }
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