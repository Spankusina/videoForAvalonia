using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using videoava.ViewModels;

namespace videoava.Views
{
    public partial class PlayerControls : UserControl
    {
        private VideoPlayerViewModel? viewModel;
        private bool isPaused;
        private Slider volumeSlider;
        private Slider timeSlider;

        public PlayerControls()
        {
            InitializeComponent();
            Loaded += OnPlayerControlsLoaded;
        }
        
        public void SetDataContext(VideoPlayerViewModel videoPlayerViewModel)
        {
            viewModel = videoPlayerViewModel;
            DataContext = viewModel;
        }

        private void OnPlayerControlsLoaded(object? sender, RoutedEventArgs e)
        {
            if (viewModel == null)
                throw new Exception($"Couldn't find the {nameof(VideoPlayerViewModel)}. " +
                                    $"Did you call the {nameof(SetDataContext)} method?");
            
            volumeSlider = this.Get<Slider>("SliderVolume");
            timeSlider = this.Get<Slider>("SliderTime");

            volumeSlider.Value = 90.0;
            viewModel.XVolume = 90.0;

            volumeSlider.AddHandler(PointerPressedEvent, VolumeSlider_PointerPressed, RoutingStrategies.Tunnel);
            volumeSlider.AddHandler(PointerReleasedEvent, VolumeSlider_PointerReleased, RoutingStrategies.Tunnel);
            
            timeSlider.AddHandler(PointerReleasedEvent, TimeSlider_PointerReleased, RoutingStrategies.Tunnel);
            timeSlider.AddHandler(PointerPressedEvent, TimeSlider_PointerPressed, RoutingStrategies.Tunnel);
        }

        private void VolumeSlider_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (viewModel == null)
                return;
            
            viewModel.XVolume = volumeSlider.Value;
        }

        private void VolumeSlider_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (viewModel == null)
                return;
            
            viewModel.XVolume = volumeSlider.Value;
        }
        
        private void TimeSlider_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            viewModel?.Pause();
        }
        
        private void TimeSlider_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (viewModel == null)
                return;
            
            if (viewModel.VideoDuration == 0)
                viewModel.ChangeVideoDuration();
            
            var duration = viewModel.VideoDuration;

            var time = (long)Math.Ceiling((timeSlider.Value * duration) / 100);
            
            viewModel.ChangeTime(time);
            viewModel.Pause();
        }
        
        private void PlayerPause(object sender, RoutedEventArgs e)
        {
            var icon = (Button)sender;
            icon.Classes.Clear();

            switch (isPaused)
            {
                case true:
                    icon.Classes.Add("Pause");
                    isPaused = false;
                    break;
                case false:
                    icon.Classes.Add("Play");
                    isPaused = true;
                    break;
            }
            
            viewModel?.Pause();
        }
    }
}
