namespace NativeMediaMauiLib.Platforms.Android;

public delegate void StatusChangedEventHandler(object sender, EventArgs e);

public delegate void BufferingEventHandler(object sender, EventArgs e);

public delegate void CoverReloadedEventHandler(object sender, EventArgs e);

public delegate void PlayingEventHandler(object sender, EventArgs e);

public delegate void PlayingChangedEventHandler(object sender, bool e);

public delegate void ErrorEventHandler(object sender, EventArgs e);

public delegate void CompletionEventHandler(object sender, EventArgs e);

public delegate void PlayedEventHandler(object sender, EventArgs e);
public delegate void PausedEventHandler(object sender, EventArgs e);
public delegate void StoppedEventHandler(object sender, EventArgs e);
public delegate void SkipToNextEventHandler(object sender, EventArgs e);
public delegate void SkipToPreviousEventHandler(object sender, EventArgs e);