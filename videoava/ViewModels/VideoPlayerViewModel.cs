using System;
using Avalonia.Controls;
using Avalonia.Threading;
using LibVLCSharp.Avalonia.Unofficial;
using LibVLCSharp.Shared;
using ReactiveUI;

namespace videoava.ViewModels;

public class VideoPlayerViewModel : ReactiveObject
{
    private VideoView? videoViewer;
    private MediaPlayer mediaPlayer;
    private LibVLC libVlc;

    private double _volume;
    private double _instantTimeValue;
    private double _duration;
    private readonly string[] _mediaAdditionalOptions = Array.Empty<string>();
    
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
    
    public VideoPlayerViewModel()
    {
        if (Design.IsDesignMode)
            throw new Exception("Design mode is not supported");

        Core.Initialize();
        libVlc = new LibVLC(enableDebugLogs: true);
        mediaPlayer = new MediaPlayer(libVlc);
        mediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
        mediaPlayer.Playing += MediaPlayer_Playing;
        mediaPlayer.EndReached += MediaPlayer_EndReached;
    }

    public void InitVideo(VideoView videoView)
    {
        videoViewer = videoView;
        videoViewer.MediaPlayer = mediaPlayer;
        PlayVideo();
    }

    public void ChangeVideoDuration()
    {
        VideoDuration = mediaPlayer.Length;
    }
    
    public void Pause()
    {
        mediaPlayer.Pause();
    }
    
    public void ChangeTime(long time)
    {
        try
        {
            mediaPlayer.Time = time;
        }
        catch
        {
            // ignored
        }
    }
    
    private void PlayVideo()
    {
        if (videoViewer == null)
            throw new Exception("VideoViewer is null!");
        
        var media = GetMediaFromUrl("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
        mediaPlayer.SetHandle(videoViewer.hndl);
        mediaPlayer.Play(media);
    }
    
    private Media GetMediaFromUrl(string url)
    {
        var mediaVideo = new Media(
            libVlc, 
            new Uri(url),
            _mediaAdditionalOptions
        );

        return mediaVideo;
    }
    
    private void MediaPlayer_TimeChanged(object? sender, MediaPlayerTimeChangedEventArgs e)
    {
        mediaPlayer.Volume = (int)Math.Ceiling(XVolume);
        try
        { 
            XTime = mediaPlayer.Time * 100 / VideoDuration;
        }
        catch 
        { 
            // ignored
        }
    }
    
    private void MediaPlayer_Playing(object? sender, EventArgs e)
    {
        try
        {
            VideoDuration = mediaPlayer.Length;
        }
        catch
        {
            // ignored
        }
    }

    private void MediaPlayer_EndReached(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(PlayVideo);
    }
}