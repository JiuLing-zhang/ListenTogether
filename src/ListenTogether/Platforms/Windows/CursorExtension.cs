using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using System.Reflection;
using Windows.UI.Core;

namespace ListenTogether.HandCursorControls;
internal static class CursorExtension
{
    private static InputCursor HandCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Hand, 0));
    public static void SetHandCursor(this UIElement uiElement)
    {
        Type type = typeof(UIElement);
        type.InvokeMember("ProtectedCursor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, uiElement, new object[] { HandCursor });
    }
}