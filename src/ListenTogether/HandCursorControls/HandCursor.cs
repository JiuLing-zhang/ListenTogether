namespace ListenTogether.HandCursorControls;
/// <summary>
/// “手” 形光标
/// </summary>
internal class HandCursor
{
    private HandCursor()
    {

    }

    /// <summary>
    /// 为组件绑定光标
    /// </summary>
    public static void Binding()
    {
#if WINDOWS
        Microsoft.Maui.Handlers.BorderHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
            if (view is HandCursorBorder)
            {
                handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

            }
        });
        Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
            if (view is HandCursorButton)
            {
                handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

            }
        });
        Microsoft.Maui.Handlers.LayoutHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
            if (view is HandCursorStackLayout)
            {
                handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

            }
        });
        Microsoft.Maui.Handlers.ImageHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
            if (view is HandCursorImage)
            {
                handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

            }
        });
        Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
            if (view is HandCursorLabel)
            {
                handler.PlatformView.PointerEntered += (_, __) => handler.PlatformView.SetHandCursor();

            }
        });
#endif
    }
}