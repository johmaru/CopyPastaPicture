using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CopyPastaPicture.core.lang;
using CopyPastaPicture.core.lib;
using Tommy;

namespace CopyPastaPicture.core.page;

public partial class SettingPage : Page
{
    TomlControl _tomlControl = new();
    LogController _logController = new();
    String _initLanguage = "";
    public SettingPage()
    {
        InitializeComponent();
        _logController.Initialize();
        LanguageLoaded();
    }

    private void LanguageLoaded()
    {
        switch (_tomlControl.LanguageName())
        {
            case "en-US":
                SaveButton.Content = EnLanguage.save;
                CancelButton.Content = EnLanguage.cancel;
                TitleLabel.Content = EnLanguage.Setting;
                LanguageLabel.Content = EnLanguage.LanguageSetting;
                EnglishComboBoxItem.Content = EnLanguage.LanguageEnglish;
                JapaneseComboBoxItem.Content = EnLanguage.LanguageJapanese;
                CliModeLabel.Content = EnLanguage.CliMode;
                break;
            case "ja-JP":
                SaveButton.Content = JaLanguage.save;
                CancelButton.Content = JaLanguage.cancel;
                TitleLabel.Content = JaLanguage.Setting;
                LanguageLabel.Content = JaLanguage.LanguageSetting;
                EnglishComboBoxItem.Content = JaLanguage.LanguageEnglish;
                JapaneseComboBoxItem.Content = JaLanguage.LanguageJapanese;
                CliModeLabel.Content = JaLanguage.CliMode;
                break;
        }

        _initLanguage = _tomlControl.LanguageName();
    }

    public void SaveContent()
    {
        //言語設定
        switch (LanguageCombo.SelectedIndex)
        {
            case 0:
                TomlEditLanguage("en-US");
                Console.WriteLine("en-US");
                break;
            case 1:
                TomlEditLanguage("ja-JP");
                Console.WriteLine("ja-JP");
                break;
        }
        
        //セーブ処理後の処理
        LanguageLoaded();
        ((App)Application.Current).LoadNotifyIcon();
        var mainPage = ((App)Application.Current).MainPageInstance;
        mainPage.InitializeLanguage();
        if (CliModeToggleSwitch.IsOn)
        {
            mainPage.ReloadContent(true);
        }
        else
        {
            mainPage.ReloadContent(false);
        }
        _logController.InfoLog("JapaneseComboBoxItem Success");
    }

    private int CheckSavedItem()
    {
        int result;
        string language = "";
        // 言語変更確認
        switch (LanguageCombo.SelectedIndex)
        {
            case 0 :
                language = "en-US";
                break;
            case 1:
                language = "ja-JP";
                break;
        }

        result = _initLanguage == language ? 0 : 1;
        return result;
    }
    
    private void TomlEditLanguage(string data)
    {
        try
        {
            using (StreamReader reader = new StreamReader(File.OpenRead("./Data/Setting.toml")))
            {
                var toml = TOML.Parse(reader);
                toml["Lang"] =data;
                using (StreamWriter writer = new StreamWriter(File.OpenWrite("./Data/Setting.toml")))
                {
                    toml.WriteTo(writer);
                    writer.Flush();
                }
            }
            _logController.InfoLog("TomlEditLanguage Success");
        }
        catch (Exception e)
        {
            _logController.ErrorLog($"TomlEditLanguage Error{e}");
            throw;
        }
    }

    private void LanguageCombo_OnLoaded(object sender, RoutedEventArgs e)
    {
        switch (_tomlControl.LanguageName())
        {
            case "en-US":
                LanguageCombo.SelectedItem = EnglishComboBoxItem;
                break;
            case "ja-JP":
                LanguageCombo.SelectedItem = JapaneseComboBoxItem;
                break;
        }
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
         if (CheckSavedItem() == 1)
         {
             _logController.InfoLog("Cancel SettingPage Success");
             switch (_tomlControl.LanguageName())
             {
                    case "ja-JP":
                        var resultJa = MessageBox.Show("変更されている設定があります、本当にキャンセルしますか？", "CopyPastaPicture",
                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                        switch (resultJa)
                        {
                            case MessageBoxResult.No:
                                return;
                            case MessageBoxResult.Yes:
                            {
                                var parentWindowJa = Window.GetWindow(this);
                                parentWindowJa?.Close();
                                break;
                            }
                        }
                        break;
                        case "en-US":
                        var resultEn = MessageBox.Show("There are some settings that have been changed, are you sure you want to cancel?", "CopyPastaPicture",
                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                        switch (resultEn)
                        {
                            case MessageBoxResult.No:
                                return;
                            case MessageBoxResult.Yes:
                            {
                                var parentWindowEn = Window.GetWindow(this);
                                parentWindowEn?.Close();
                                break;
                            }
                        }    
                            break;
             }
         }
         else
         {
                _logController.InfoLog("Cancel SettingPage Success");
                var parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
         }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        SaveContent();
    }

    private void CliModeToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
    {
        if (CliModeToggleSwitch.IsOn)
        {
            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead("./Data/Setting.toml")))
                {
                    var toml = TOML.Parse(reader);
                    toml["CliMode"] = "true";
                    using (StreamWriter writer = new StreamWriter(File.OpenWrite("./Data/Setting.toml")))
                    {
                        toml.WriteTo(writer);
                        writer.Flush();
                    }
                }
                _logController.InfoLog("SettingCliMode Change Success");
            }
            catch (Exception exception)
            {
                _logController.ErrorLog($"SettingCliMode Change Error{exception}");
                throw;
            }
        }
        else
        {
            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead("./Data/Setting.toml")))
                {
                    var toml = TOML.Parse(reader);
                    toml["CliMode"] = "false";
                    using (StreamWriter writer = new StreamWriter(File.OpenWrite("./Data/Setting.toml")))
                    {
                        toml.WriteTo(writer);
                        writer.Flush();
                    }
                }
                _logController.InfoLog("SettingCliMode Change Success");
            }
            catch (Exception exception)
            {
                _logController.ErrorLog($"SettingCliMode Change Error{exception}");
                throw;
            }
        }
    }

    private void CliModeToggleSwitch_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_tomlControl.GetTomlData("CliMode").Equals("true"))
        {
            CliModeToggleSwitch.IsOn = true;
        }
        else if (_tomlControl.GetTomlData("CliMode").Equals("false"))
        {
            CliModeToggleSwitch.IsOn = false;
        }
    }
}