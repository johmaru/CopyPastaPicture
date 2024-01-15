using System.Windows;
using CopyPastaPicture.core.lib;
using CopyPastaPicture.core.page;

namespace CopyPastaPicture.core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TomlControl _tomlControl = new();
        private LogController _logController = new();
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            NavigateToPage();
        }

        private void Initialize()
        {
            _tomlControl.Initialize();
            _logController.Initialize();
            _logController.InfoLog("Initialize MainWindow");
        }

        private void NavigateToPage()
        {
            Frame.NavigationService.Navigate(new MainPage());
            _logController.InfoLog("Move NavigateToPage Success");
        }
        
    }
}