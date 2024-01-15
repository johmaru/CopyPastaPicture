using System.Windows.Controls;
using CopyPastaPicture.core.lang;
using CopyPastaPicture.core.lib;

namespace CopyPastaPicture.core.page;

public partial class SettingPage : Page
{
    TomlControl _tomlControl = new();
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
                break;
            case "ja-JP":
                SaveButton.Content = JaLanguage.save;
                CancelButton.Content = JaLanguage.cancel;
                break;
        }
    }
}