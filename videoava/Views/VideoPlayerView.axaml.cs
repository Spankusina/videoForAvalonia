using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using videoava.ViewModels;
using LibVLCSharp.Avalonia.Unofficial;

namespace videoava.Views;

public partial class VideoPlayerView : UserControl
{
    private readonly VideoView videoViewer;
    private PlayerControls playerControls;
    
    public VideoPlayerView()
    {
        InitializeComponent();

        videoViewer = this.Get<VideoView>("VideoViewer");
        playerControls = this.Get<PlayerControls>("PlayerControlsInView");
  
        Loaded += MainWindow_Opened;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        if (DataContext is not VideoPlayerViewModel viewModel)
            throw new Exception("Couldn't find the VideoPlayerViewModel");

        playerControls.SetDataContext(viewModel);
        viewModel.InitVideo(videoViewer);
    }
}