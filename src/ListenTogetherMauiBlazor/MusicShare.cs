using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class MusicShare : IMusicShare
{
    public async Task ShareLinkAsync(string url, string musicInfo)
    {
        if (Config.Desktop)
        {
            await Clipboard.Default.SetTextAsync(url);
            await App.Current.MainPage.DisplayAlert("提示", $"{musicInfo}{Environment.NewLine}歌曲链接已复制", "确定");
        }
        else
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = url,
                Title = $"分享歌曲链接{Environment.NewLine}{musicInfo}"
            });
        }
    }
}