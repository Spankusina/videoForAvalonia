using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using videoava.ViewModels;
using LibVLCSharp.Avalonia.Unofficial;
using Tmds.DBus;

namespace videoava.Views;

public partial class VideoPlayer : UserControl
{
    private static VideoPlayer? _this;
    public VideoView _videoViewer;
    public MainWindowViewModel ViewModel;
    
    public VideoPlayer()
    {
        InitializeComponent();
        
        ViewModel = new MainWindowViewModel();
        /*DataContext = ViewModel;*/

        _videoViewer = this.Get<VideoView>("VideoViewer");
  
        _this = this;

        Loaded += MainWindow_Opened;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static VideoPlayer GetInstance()
    {            
        return _this;            
    }

    private void MainWindow_Opened(object? sender, System.EventArgs e)
    {
        /*if (ViewModel.MediaPlayer != null)
        {               
            _videoViewer.MediaPlayer = ViewModel.MediaPlayer;                
            _videoViewer.MediaPlayer.SetHandle(_videoViewer.hndl);

            var media = ViewModel.MediaVideo;
                
            ViewModel.MediaPlayer.Play(media);
        }*/

        ViewModel.PlayVideo(_videoViewer);
    }
}