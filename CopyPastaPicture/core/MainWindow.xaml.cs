using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using CopyPastaPicture.core.lib;
using CopyPastaPicture.core.page;
using Tommy;

namespace CopyPastaPicture.core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LogController _logController = new();
        private int _width;
        private int _height;
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            SetWindowResolution();
            NavigateToPage();
        }

        private void Initialize()
        {
            _logController.InfoLog("Initialize MainWindow");
        }

        private void NavigateToPage()
        {
            Frame.NavigationService.Navigate(new MainPage());
            _logController.InfoLog("Move NavigateToPage Success");
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            _logController.InfoLog("Close MainWindow");
        }

        private void SetWindowResolution()
        {
            try
            {
                using (StreamReader reader = File.OpenText(TomlControl.TomlMain.TomlDataDir))
                {
                    TomlTable table = TOML.Parse(reader);

                    var width = table["WindowResolution"]["Width"];
                    var height = table["WindowResolution"]["Height"];

                    // intに変換する必要がある
                    _width = width;
                    _height = height;
                    
                    this.Width = _width;
                    this.Height = _height;
                    _logController.InfoLog("SetWindowResolution Success");
                }
            }
            catch (Exception e)
            {
                _logController.ErrorLog($"SetWindowResolution Error : {e}");
                Console.WriteLine(e);
                throw;
            }
        }
    }
}