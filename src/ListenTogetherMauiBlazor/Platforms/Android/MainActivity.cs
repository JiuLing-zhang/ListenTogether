﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using NativeMediaMauiLib.Platforms.Android;
using NativeMediaMauiLib.Platforms.Android.CurrentActivity;

namespace ListenTogetherMauiBlazor
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity, IAudioActivity
    {
        MediaPlayerServiceConnection mediaPlayerServiceConnection;

        public MediaPlayerServiceBinder Binder { get; set; }

        public event StatusChangedEventHandler StatusChanged;
        public event CoverReloadedEventHandler CoverReloaded;
        public event PlayingEventHandler Playing;
        public event BufferingEventHandler Buffering;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            NotificationHelper.CreateNotificationChannel(ApplicationContext);
            if (mediaPlayerServiceConnection == null)
                InitializeMedia();

            var intent = new Intent(ApplicationContext, typeof(MediaPlayerService));
            ApplicationContext.StartForegroundService(intent);
        }

        private void InitializeMedia()
        {
            mediaPlayerServiceConnection = new MediaPlayerServiceConnection(this);
            var mediaPlayerServiceIntent = new Intent(ApplicationContext, typeof(MediaPlayerService));
            BindService(mediaPlayerServiceIntent, mediaPlayerServiceConnection, Bind.AutoCreate);
        }
    }
}