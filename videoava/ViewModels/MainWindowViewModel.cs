using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using LibVLCSharp.Shared;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Avalonia.Interactivity;
using videoava.Views;
using LibVLCSharp.Avalonia.Unofficial;

namespace videoava.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public LibVLC? LibVlc;
        public static MediaPlayer? MediaPlayer;
        public static Media? MediaVideo;
        private double _volume;
        private double _instantTimeValue;
        private double _duration;
        private readonly string[] _mediaAdditionalOptions = { };

        public MainWindowViewModel()
        {
            if (!Avalonia.Controls.Design.IsDesignMode)
            {
                {
                    Core.Initialize();
                }
                LibVlc = new LibVLC(enableDebugLogs: true);
                InitializePlayer();
                
                MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
                MediaPlayer.Playing += MediaPlayer_Playing;
                MediaPlayer.EndReached += MediaPlayer_EndReached;
            }
        }

        public double XVolume
        {
            get => _volume;
            set => this.RaiseAndSetIfChanged(ref _volume, value);
        }
        
        public double XTime
        {
            get => _instantTimeValue;
            set => this.RaiseAndSetIfChanged(ref _instantTimeValue, value);
        }
        
        public double VideoDuration
        {
            get => _duration;
            set => this.RaiseAndSetIfChanged(ref _duration, value);
        }

        public void InitializePlayer()
        {
            MediaPlayer = new MediaPlayer(LibVlc) {};
                
            MediaVideo = new Media(
                LibVlc, 
                new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"),
                _mediaAdditionalOptions
            );
        }

        public void PlayVideo(VideoView videoViewer)
        {
            if (MediaPlayer != null)
            {               
                videoViewer.MediaPlayer = MediaPlayer;                
                videoViewer.MediaPlayer.SetHandle(videoViewer.hndl);

                MediaPlayer.Play(MediaVideo);
            }
        }

        public void ChangeVideoDuration()
        {
            VideoDuration = MediaPlayer.Length;
        }
        
        public void Pause()
        {
            if (MediaPlayer != null)
            {
                MediaPlayer.Pause();
            }
        }
        
        public void ChangeTime(long time)
        {
            try
            {
                MediaPlayer.Time = time;
            }
            catch { }
        }
        
        private void MediaPlayer_TimeChanged(object? sender, MediaPlayerTimeChangedEventArgs e)
        {
            var playerControlsInstance = PlayerControls.GetInstance();
            MediaPlayer.Volume = (int)Math.Ceiling(playerControlsInstance.ViewModel.XVolume);
            try
            {
                playerControlsInstance.ViewModel.XTime = (MediaPlayer.Time * 100) / VideoDuration;
            }
            catch { }

        }
        
        private void MediaPlayer_Playing(object? sender, EventArgs e)
        {
            try
            {
                VideoDuration = MediaPlayer.Length;
            }
            catch { }
        }

        private void MediaPlayer_EndReached(object? sender, EventArgs e)
        {
            //Executed after the video has played to the end
            
            InitializePlayer();

            var videoPlayerInstance = VideoPlayer.GetInstance();
            var videoViewer = videoPlayerInstance._videoViewer;
            PlayVideo(videoViewer);
        }
    }
}
