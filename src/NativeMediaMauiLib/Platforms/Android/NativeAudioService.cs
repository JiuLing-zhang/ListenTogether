using Android.Graphics;
using Android.Media;
using AndroidApp = Android.App;

namespace NativeMediaMauiLib.Platforms.Android
{
    public class NativeAudioService : INativeAudioService
    {
        IAudioActivity instance;

        private MediaPlayer mediaPlayer => instance != null &&
            instance.Binder.GetMediaPlayerService() != null ?
            instance.Binder.GetMediaPlayerService().mediaPlayer : null;

        public bool IsPlaying => mediaPlayer?.IsPlaying ?? false;

        public double CurrentPositionMillisecond => mediaPlayer?.CurrentPosition ?? 0;

        public double CurrentDurationMillisecond => mediaPlayer?.Duration ?? 0;

        public event EventHandler<bool> IsPlayingChanged;
        public event EventHandler PlayFinished;
        public event EventHandler PlayFailed;

        public event EventHandler Played;
        public event EventHandler Paused;
        public event EventHandler Stopped;
        public event EventHandler SkipToNext;
        public event EventHandler SkipToPrevious;

        public Task InitializeAsync(string audioURI, AudioMetadata audioMetadata)
        {
            if (instance == null)
            {
                var activity = CurrentActivity.CrossCurrentActivity.Current;
                instance = activity.Activity as IAudioActivity;

                instance.Binder.GetMediaPlayerService().OnPlayerPlay += (_, _) => Played?.Invoke(this, EventArgs.Empty);
                instance.Binder.GetMediaPlayerService().OnPlayerPause += (_, _) => Paused?.Invoke(this, EventArgs.Empty);
                instance.Binder.GetMediaPlayerService().OnPlayerSkipToNext += (_, _) => SkipToNext?.Invoke(this, EventArgs.Empty);
                instance.Binder.GetMediaPlayerService().OnPlayerSkipToPrevious += (_, _) => SkipToPrevious?.Invoke(this, EventArgs.Empty);
                instance.Binder.GetMediaPlayerService().OnPlayerError += (_, _) => PlayFailed?.Invoke(this, EventArgs.Empty);
                instance.Binder.GetMediaPlayerService().OnPlayerCompletion += (_, _) => PlayFinished?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                instance.Binder.GetMediaPlayerService().isCurrentEpisode = false;
                instance.Binder.GetMediaPlayerService().UpdatePlaybackStateStopped();
            }

            instance.Binder.GetMediaPlayerService().AudioUrl = audioURI;
            instance.Binder.GetMediaPlayerService().NotificationInfo = new NotificationInfo(
               BitmapFactory.DecodeByteArray(audioMetadata.Image, 0, audioMetadata.Image.Length),
               audioMetadata.Name,
               audioMetadata.Artist,
               audioMetadata.Album);

            return Task.CompletedTask;
        }

        public Task PauseAsync()
        {
            if (IsPlaying)
            {
                return instance.Binder.GetMediaPlayerService().Pause();
            }

            return Task.CompletedTask;
        }

        public async Task PlayAsync(double positionMillisecond = 0)
        {
            await instance.Binder.GetMediaPlayerService().Play();
            await instance.Binder.GetMediaPlayerService().Seek((int)positionMillisecond);
        }

        public Task SetMuted(bool value)
        {
            instance?.Binder.GetMediaPlayerService().SetMuted(value);

            return Task.CompletedTask;
        }

        public Task SetVolume(int value)
        {
            instance?.Binder.GetMediaPlayerService().SetVolume(value);

            return Task.CompletedTask;
        }

        public Task SetCurrentTime(double position)
        {
            return instance.Binder.GetMediaPlayerService().Seek((int)position * 1000);
        }

        public ValueTask DisposeAsync()
        {
            instance.Binder?.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
