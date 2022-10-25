namespace NativeMediaMauiLib.Platforms.Android;

public delegate void StatusChangedEventHandler(object sender, EventArgs e);

public delegate void BufferingEventHandler(object sender, EventArgs e);

public delegate void CoverReloadedEventHandler(object sender, EventArgs e);

public delegate void PlayingEventHandler(object sender, EventArgs e);

public delegate void PlayingChangedEventHandler(object sender, bool e);

public delegate void OnPlayerPlayEventHandler(object sender, EventArgs e);
public delegate void OnPlayerPauseEventHandler(object sender, EventArgs e);
public delegate void OnPlayerSkipToNextEventHandler(object sender, EventArgs e);
public delegate void OnPlayerSkipToPreviousEventHandler(object sender, EventArgs e);
public delegate void OnPlayerErrorEventHandler(object sender, EventArgs e);
public delegate void OnPlayerCompletionEventHandler(object sender, EventArgs e);
