using ReactiveUI;

namespace videoava.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public VideoPlayerViewModel VideoPlayerViewModel { get; }
        
        public MainWindowViewModel()
        {
            VideoPlayerViewModel = new();
        }
    }
}
