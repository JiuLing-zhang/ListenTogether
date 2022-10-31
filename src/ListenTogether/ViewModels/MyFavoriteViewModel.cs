using CommunityToolkit.Mvvm.ComponentModel;

namespace ListenTogether.ViewModels;
public partial class MyFavoriteViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name = null!;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ImageByteArray))]
    private string _imageUrl = null!;

    public byte[] ImageByteArray => ImageUrl.IsEmpty() ? null : ImageCacheUtils.GetByteArrayUsingCache(ImageUrl);

    [ObservableProperty]
    private int _musicCount;

    public DateTime EditTime { get; set; }
}