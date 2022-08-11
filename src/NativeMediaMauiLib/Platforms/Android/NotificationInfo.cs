using Android.Graphics;

namespace NativeMediaMauiLib.Platforms.Android;
public class NotificationInfo
{
    public Bitmap SmallIcon { get; set; }
    public Bitmap LargeIcon { get; set; }
    public string ContentTitle { get; set; }
    public string ContentText { get; set; }
    public string SubText { get; set; }

    public NotificationInfo(Bitmap smallIcon, Bitmap largeIcon, string contentTitle, string contentText, string subText)
    {
        SmallIcon = smallIcon;
        LargeIcon = largeIcon;
        ContentTitle = contentTitle;
        ContentText = contentText;
        SubText = subText;
    }
}