using Android.Graphics;

namespace NativeMediaMauiLib.Platforms.Android;
public class NotificationInfo
{
    public Bitmap Icon { get; set; }
    public string ContentTitle { get; set; }
    public string ContentText { get; set; }
    public string SubText { get; set; }

    public NotificationInfo(Bitmap icon, string contentTitle, string contentText, string subText)
    {
        Icon = icon;
        ContentTitle = contentTitle;
        ContentText = contentText;
        SubText = subText;
    }
}