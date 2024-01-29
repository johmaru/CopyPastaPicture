using System.Windows;
using System.Windows.Controls;
using CopyPastaPicture.core.lang;
using CopyPastaPicture.core.lib;
using CopyPastaPicture.core.window;

namespace CopyPastaPicture.core.page;

public partial class MainPage : Page
{
   private LogController _logController = new();
   private TomlControl _tomlControl = new();
    public MainPage()
    {
        InitializeComponent();
        _logController.Initialize();
        Initialize();
        InitializeLanguage();
    }

    private void Initialize()
    {
        _logController.InfoLog("Initialize MainPage");
    }
    public void InitializeLanguage()
    {
        switch (_tomlControl.LanguageName())
        {
            case "en-US":
                CliButton.Header = EnLanguage.OpenCliButton;
                SettingButton.Header = EnLanguage.OpenSettingButton;
                ExitButton.Header = EnLanguage.Exit;
                HelpButton.Header = EnLanguage.Help;
                FileItem.Header = EnLanguage.File;
                break;
            case "ja-JP":
                CliButton.Header = JaLanguage.OpenCliButton;
                SettingButton.Header = JaLanguage.OpenSettingButton;
                ExitButton.Header = JaLanguage.Exit;
                HelpButton.Header = JaLanguage.Help;
                FileItem.Header = JaLanguage.File;
                break;
        }
    }

    public void ReloadContent(bool cliCheck)
    {
        CliButton.Visibility = cliCheck is true ? Visibility.Visible : Visibility.Hidden;
    }

    private void CliButton_OnClick(object sender, RoutedEventArgs e)
    {
        CliWindow cliWindow = new();
        cliWindow.Owner = Window.GetWindow(this);
        cliWindow.Show();
    }

    private void SettingButton_OnClick(object sender, RoutedEventArgs e)
    {
       SettingWindow settingWindow = new();
            settingWindow.Owner = Window.GetWindow(this);
            settingWindow.Show();
    }

    private void HelpButton_OnClick(object sender, RoutedEventArgs e)
    {
        switch (_tomlControl.LanguageName())
        {
            case "en-US":
                MessageBox.Show(EnLanguage.HelpCommand, EnLanguage.HelpTitle, MessageBoxButton.OK);
                break;
            case "ja-JP":
                MessageBox.Show(JaLanguage.HelpCommand, JaLanguage.HelpTitle, MessageBoxButton.OK);
                break;
        }
    }

    private void ExitButton_OnClick(object sender, RoutedEventArgs e)
    {
        var parent = Window.GetWindow(this);
        parent?.Close();
    }
}