using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using CopyPastaPicture.core.lang;
using CopyPastaPicture.core.lib;

namespace CopyPastaPicture.core.window.subwindow;

public partial class TextEditWindow : Window
{
    LogController _logController = new();
    private readonly string _text;
    private readonly string _dir;
    private TomlControl _tomlControl = new();
    public TextEditWindow(string dir,string text)
    {
        InitializeComponent();
        Initialize();
        _text = text;
        _dir = dir;
        Task.Run(() => LanguageInit());
        Task.Run(() => EditTextInit());
    }
    
    private void Initialize()
    {
        _logController.Initialize();
        _logController.InfoLog("Initialize TextEditWindow");
    }
    
    private async Task LanguageInit()
    {
        switch (_tomlControl.LanguageName())
        {
            case "ja-JP":
                await Dispatcher.InvokeAsync(() => SaveButton.Content = JaLanguage.save);
                await Dispatcher.InvokeAsync(() => CancelButton.Content = JaLanguage.cancel);
                await Dispatcher.InvokeAsync(() => Title = "テキスト編集");
                break;
            case "en-US":
                await Dispatcher.InvokeAsync(() => SaveButton.Content = EnLanguage.save);
                await Dispatcher.InvokeAsync(() => CancelButton.Content = EnLanguage.cancel);
                await Dispatcher.InvokeAsync(() => Title = "TextEdit");
                break;
        }
        _logController.InfoLog("LanguageInit");
    }
    private async Task EditTextInit()
    {
        await Dispatcher.InvokeAsync(() => EditText.Text = _text); 
        _logController.InfoLog("EditTextInit");
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    { 
        string dir = _dir;
        File.AppendAllText($"{dir}", EditText.Text);
        _logController.InfoLog("Save TextEditWindow");
        this.Close();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_text == EditText.Text) return;
        switch (_tomlControl.LanguageName())
        {
            case "ja-JP":
                if (MessageBox.Show("テキストが変更されています、本当にキャンセルしますか？", "キャンセル", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _logController.InfoLog("Cancel TextEditWindow");
                    this.Close();
                }
                break;
            case "en-US":
                if (MessageBox.Show("The text has been changed, are you sure you want to cancel?", "Cancel", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _logController.InfoLog("Cancel TextEditWindow");
                    this.Close();
                }
                break;
        }
    }
}