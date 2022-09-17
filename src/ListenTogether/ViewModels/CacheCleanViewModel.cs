using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ListenTogether.ViewModels;

public partial class CacheCleanViewModel : ViewModelBase
{
    public CacheCleanViewModel()
    {
        Caches = new ObservableCollectionEx<MusicFileViewModel>();
        Caches.ItemChanged += Caches_ItemChanged;
        Caches.CollectionChanged += Caches_CollectionChanged;
    }
    public async Task InitializeAsync()
    {
        try
        {
            StartLoading("");
            SelectedSize = 0;
            await GetCacheFilesAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("缓存文件查找失败");
            Logger.Error("缓存文件查找失败。", ex);
        }
        finally
        {
            StopLoading();
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

    private async Task GetCacheFilesAsync()
    {
        Caches.Clear();
        AllSize = 0;
        await EachDirectoryAsync(GlobalConfig.MusicCacheDirectory, CalcFilesInfo);
    }

    private async Task EachDirectoryAsync(string folderPath, Action<List<string>> callbackFilePaths)
    {
        try
        {
            Directory.GetDirectories(folderPath).ToList().ForEach(async path =>
            {
                //继续遍历文件夹内容
                await EachDirectoryAsync(path, callbackFilePaths);
            });

            callbackFilePaths.Invoke(Directory.GetFiles(folderPath).ToList());
        }
        catch (UnauthorizedAccessException ex)
        {
            await ToastService.Show($"没有权限访问文件夹：{folderPath}");
        }
    }

    private void CalcFilesInfo(List<string> paths)
    {
        //根据路径加载文件信息
        var files = paths.Select(path => new FileInfo(path)).ToList();
        files.ForEach(file =>
        {
            //符合条件的文件写入队列
            var musicFile = new MusicFileViewModel()
            {
                Name = file.Name,
                FullName = file.FullName,
                Size = file.Length,
                IsChecked = false
            };
            Caches.Add(musicFile);
            AllSize = AllSize + musicFile.Size;
        });
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
    private async void ClearAsync()
    {
        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除吗？删除后不可恢复。", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            StartLoading("处理中....");
            foreach (var cache in Caches)
            {
                if (cache.IsChecked == true)
                {
                    try
                    {
                        System.IO.File.Delete(cache.FullName);
                    }
                    catch (Exception ex)
                    {
                        await ToastService.Show($"文件删除失败：{cache.FullName}");
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
            StopLoading();
        }

        await GetCacheFilesAsync();
        await ToastService.Show("删除完成");
    }

    private string SizeToString(Int64 size)
    {
        return $"{size / 1024 / 1024:N2}";
    }
}