using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels;

public class CacheCleanViewModel : ViewModelBase
{
    public ICommand SelectAllCommand => new Command(SelectAll);
    public ICommand ClearCommand => new Command(Clear);
    public CacheCleanViewModel()
    {
        Caches = new ObservableCollectionEx<MusicFileViewModel>();
        Caches.ItemChanged += Caches_ItemChanged;
    }

    public async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
            SelectedSize = 0;
            await GetCacheFiles();
        }
        catch (Exception ex)
        {
            await ToastService.Show("缓存文件查找失败");
            Logger.Error("缓存文件查找失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }

    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged("IsBusy");
            OnPropertyChanged("IsNotBusy");
        }
    }
    public bool IsNotBusy => !_isBusy;


    private Int64 _allSize;
    public Int64 AllSize
    {
        get => _allSize;
        set
        {
            _allSize = value;
            OnPropertyChanged();

            AllSizeString = SizeToString(value);
        }
    }

    private string _allSizeString;
    public string AllSizeString
    {
        get => _allSizeString;
        set
        {
            _allSizeString = value;
            OnPropertyChanged();
        }
    }

    private Int64 _selectedSize;
    public Int64 SelectedSize
    {
        get => _selectedSize;
        set
        {
            _selectedSize = value;
            OnPropertyChanged();

            SelectedSizeString = SizeToString(value);
        }
    }

    private string _selectedSizeString;
    public string SelectedSizeString
    {
        get => _selectedSizeString;
        set
        {
            _selectedSizeString = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollectionEx<MusicFileViewModel> _caches;
    /// <summary>
    /// 缓存的文件
    /// </summary>
    public ObservableCollectionEx<MusicFileViewModel> Caches
    {
        get => _caches;
        set
        {
            _caches = value;
            OnPropertyChanged();
        }
    }

    private void Caches_ItemChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

    private async Task GetCacheFiles()
    {
        Caches.Clear();
        AllSize = 0;
        await EachDirectory(GlobalConfig.MusicCacheDirectory, CalcFilesInfo);
    }

    private async Task EachDirectory(string folderPath, Action<List<string>> callbackFilePaths)
    {
        try
        {
            Directory.GetDirectories(folderPath).ToList().ForEach(async path =>
            {
                //继续遍历文件夹内容
                await EachDirectory(path, callbackFilePaths);
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
    private async void Clear()
    {
        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除吗？删除后不可恢复。", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            IsBusy = true;
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
            IsBusy = false;
        }

        await GetCacheFiles();
        await ToastService.Show("删除完成");
    }

    private string SizeToString(Int64 size)
    {
        return $"{size / 1024 / 1024:N2}";
    }
}