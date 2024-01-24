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
    public SettingPage()
    {
        InitializeComponent();
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
                break;
            case "ja-JP":
                SaveButton.Content = JaLanguage.save;
                CancelButton.Content = JaLanguage.cancel;
                TitleLabel.Content = JaLanguage.Setting;
                LanguageLabel.Content = JaLanguage.LanguageSetting;
                EnglishComboBoxItem.Content = JaLanguage.LanguageEnglish;
                JapaneseComboBoxItem.Content = JaLanguage.LanguageJapanese;
                break;
        }
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

    private void EnglishComboBoxItem_OnSelected(object sender, RoutedEventArgs e)
    {
        try
        {
            TomlEditLanguage("en-US");
            LanguageLoaded();
            ((App)Application.Current).LoadNotifyIcon();
            _logController.InfoLog("EnglishComboBoxItem Success");
        }
        catch (Exception exception)
        {
            _logController.ErrorLog($"EnglishComboBoxItem Error{exception}");
            throw;
        }
    }

    private void JapaneseComboBoxItem_OnSelected(object sender, RoutedEventArgs e)
    {
        try
        {
            TomlEditLanguage("ja-JP");
            LanguageLoaded();
            ((App)Application.Current).LoadNotifyIcon();
            _logController.InfoLog("JapaneseComboBoxItem Success");
        }
        catch (Exception exception)
        {
            _logController.ErrorLog($"JapaneseComboBoxItem Error{exception}");
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
}