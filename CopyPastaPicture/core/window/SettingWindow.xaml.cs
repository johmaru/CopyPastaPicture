using System;
using System.IO;
using System.Windows;
using CopyPastaPicture.core.lib;
using CopyPastaPicture.core.page;
using Tommy;

namespace CopyPastaPicture.core.window;

public partial class SettingWindow : Window
{
    private LogController _logController = new();
    private int _width;
    private int _height;    
    public SettingWindow()
    {
        InitializeComponent();
        _logController.Initialize();
        Initialize();
        SetWindowResolution();
        NavigateToPage();
    }

    private void Initialize()
    {
        _logController.InfoLog("Initialize SettingWindow");
    }


    private void NavigateToPage()
    {
        Frame.NavigationService.Navigate(new SettingPage());
        _logController.InfoLog("Move NavigateToPage Success");
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