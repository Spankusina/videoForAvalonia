using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using videoava.ViewModels;


namespace videoava.Views
{
    public partial class PlayerControls : UserControl
    {
        public MainWindowViewModel ViewModel = new();
        private static PlayerControls? _this;
        private bool _isPaused = false;
        private Slider _volumeSlider;
        public Slider _timeSlider;

        public PlayerControls()
        {
            InitializeComponent();
            _this = this;
            DataContext = ViewModel;
            _volumeSlider = this.Get<Slider>("SliderVolume");
            _timeSlider = this.Get<Slider>("SliderTime");

            _volumeSlider.Value = 90.0;
            ViewModel.XVolume = 90.0;

            _volumeSlider.AddHandler(PointerPressedEvent, VolumeSlider_PointerPressed, RoutingStrategies.Tunnel);
            _volumeSlider.AddHandler(PointerReleasedEvent, VolumeSlider_PointerReleased, RoutingStrategies.Tunnel);
            
            _timeSlider.AddHandler(PointerReleasedEvent, TimeSlider_PointerReleased, RoutingStrategies.Tunnel);
            _timeSlider.AddHandler(PointerPressedEvent, TimeSlider_PointerPressed, RoutingStrategies.Tunnel);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        public static PlayerControls GetInstance()
        {
            return _this;
        }

        private void VolumeSlider_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.XVolume = _volumeSlider.Value;
        }

        private void VolumeSlider_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            ViewModel.XVolume = _volumeSlider.Value;
        }
        
        private void TimeSlider_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var mainWindowInstance = VideoPlayer.GetInstance();
            mainWindowInstance.ViewModel.Pause();
        }
        
        private void TimeSlider_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            var videoPlayerInstance = VideoPlayer.GetInstance();
            if (ViewModel.VideoDuration == 0)
                ViewModel.ChangeVideoDuration();
            
            var duration = ViewModel.VideoDuration;

            var time = (long)Math.Ceiling((_timeSlider.Value * duration) / 100);
            
            videoPlayerInstance.ViewModel.ChangeTime(time);
            videoPlayerInstance.ViewModel.Pause();
        }
        
        private void PlayerPause(object sender, RoutedEventArgs e)
        {
            var icon = (Button)sender;
            icon.Classes.Clear();

            switch (_isPaused)
            {
                case true:
                    icon.Classes.Add("Pause");
                    _isPaused = false;
                    break;
                case false:
                    icon.Classes.Add("Play");
                    _isPaused = true;
                    break;
            }
            
            var mainWindowInstance = VideoPlayer.GetInstance();
            mainWindowInstance.ViewModel.Pause();
        }
    }
}
