using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CopyPastaPicture.core.lang;
using CopyPastaPicture.core.lib;
using Microsoft.VisualBasic.Logging;
using Tommy;
using Clipboard = System.Windows.Clipboard;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace CopyPastaPicture.core.window;

public partial class CliWindow : Window
{
    LogController  _logController = new();
    private int _width;
    private int _height;
    private TomlControl _tomlControl = new();
    private string _initLanguage = "";
    
    public CliWindow()
    {
        InitializeComponent();
        _logController.Initialize();
        Initialize();
        SetWindowResolution();
        InitializeLanguage();
    }

    private void Initialize()
    {
        _logController.InfoLog("Initialize CliWindow");
    }
    
    private void SetWindowResolution()
    {
        try
        {
            using (StreamReader reader = File.OpenText(TomlControl.TomlMain.TomlDataDir))
            {
                TomlTable table = TOML.Parse(reader);

                var width = table["CliWindowResolution"]["Width"];
                var height = table["CliWindowResolution"]["Height"];

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

    private void InitializeLanguage()
    {
        _initLanguage = _tomlControl.LanguageName();
        
        switch (_initLanguage)
        {
            case "en-US":
               
                break;
            case "ja-JP":
                break;
        }
    }

    private void CliMain()
    {
        string[] searchWorldsAdd = { "add", "img" };
        if (CommandBox.Text.Contains("iload"))
        {
            string text = CommandBox.Text;
            string name = text.Replace("iload", "");
            string name2 = name.Replace(" ", "");
            if (Directory.Exists($"./Data/Image/{name2}"))
            {
                try
                {
                    var files = Directory.GetFiles($"./Data/Image/{name2}");
                    foreach (var file in files)
                    {
                        Console.WriteLine(file);
                        var bitmap = new BitmapImage(new Uri("file://" + Path.GetFullPath(file)));
                        Clipboard.SetImage(bitmap);
                        System.Threading.Thread.Sleep(100);
                        _logController.InfoLog($"{file} Copy Success");
                        this.Close();
                    }
                }
                catch (Exception e)
                {
                   _logController.ErrorLog($"CliMain Error {e}");
                    throw;
                }
            } 
            else
            {
                switch (_tomlControl.LanguageName())
                {
                    case "en-US":
                        MessageBox.Show(EnLanguage.UnknownName, EnLanguage.UnknownName, MessageBoxButton.OK);
                        break;
                    case "ja-JP":
                        MessageBox.Show(JaLanguage.UnknownName, JaLanguage.UnknownName, MessageBoxButton.OK);
                        break;
                }
            
            }
        }
        if (searchWorldsAdd.Any(world => CommandBox.Text.Contains(world)))
        {
            string imgText = CommandBox.Text;
            string imgCommand = imgText.Replace("Img", "");
            string imgCommandReplace = imgCommand.Replace(" ", "");
            string imgCommandReplaced = imgCommandReplace.Replace("add", "");
                    
            Console.WriteLine(imgCommandReplaced);
                
            try
            {
                if (Directory.Exists("./Data/Image"))
                {
                    switch (_tomlControl.LanguageName())
                    {
                            
                        case "en-US":
                            using (var file = new OpenFileDialog())
                            {
                                    
                                file.Title = EnLanguage.PictureSelect;
                                file.FileName = EnLanguage.ExamplePictureName;
                                file.Filter = "Image File(*.png, *.jpg, *.jpeg, *.gif)|*.png;*.jpg;*.jpeg;*.gif";
                                file.CheckFileExists = false;

                                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    try
                                    {
                                        var fi = new FileInfo(file.FileName);
                                        string path = file.SafeFileName;
                                            
                                        Directory.CreateDirectory($"./Data/Image/{imgCommandReplaced}");
                                        if (File.Exists($"./Data/Image/{imgCommandReplaced}")) return;
                                        fi.CopyTo(($"./Data/Image/{imgCommandReplaced}/{path}"));
                                        _logController.InfoLog($"File Move Success to ./Data/Image/{path}/{imgCommandReplaced}");
                                    }
                                    catch (Exception e)
                                    {
                                        _logController.ErrorLog($"File Move Error{e}");
                                        throw;
                                    } 
                                }
                            } 
                            _logController.InfoLog("CliMain Success");
                            this.Close();
                            break;
                        case "ja-JP":
                            using (var file = new OpenFileDialog())
                            {
                                    
                                file.Title = JaLanguage.PictureSelect;
                                file.FileName = JaLanguage.ExamplePictureName;
                                file.Filter = "Image File(*.png, *.jpg, *.jpeg, *.gif)|*.png;*.jpg;*.jpeg;*.gif";
                                file.CheckFileExists = false;

                                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    try
                                    {
                                        var fi = new FileInfo(file.FileName);
                                        string path = file.SafeFileName;
                                            
                                        Directory.CreateDirectory($"./Data/Image/{imgCommandReplaced}");
                                        if (File.Exists($"./Data/Image/{imgCommandReplaced}")) return;
                                        fi.CopyTo(($"./Data/Image/{imgCommandReplaced}/{path}"));
                                        _logController.InfoLog($"File Move Success to ./Data/Image/{path}/{imgCommandReplaced}");
                                    }
                                    catch (Exception e)
                                    {
                                        _logController.ErrorLog($"File Move Error{e}");
                                        throw;
                                    }
                                }
                            } 
                            _logController.InfoLog("CliMain Success");
                            this.Close();
                            break;
                    }
                }
                else
                {
                    Directory.CreateDirectory("./Data/Image");
                    _logController.InfoLog("Create Image Directory Success");
                    switch (_tomlControl.LanguageName())
                    {
                        case "en-US":
                            MessageBox.Show(EnLanguage.CreateImageDir, EnLanguage.CreateImageDirTitle, MessageBoxButton.OK);
                            break;
                        case "ja-JP":
                            MessageBox.Show(JaLanguage.CreateImageDir, JaLanguage.CreateImageDirTitle, MessageBoxButton.OK);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
               
        }
        else
        {
             switch (CommandBox.Text)
        {
            case "help":
                CliHelp();
                break;
        }
        }
       
    }

    private void CliHelp()
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

    private void CliWindow_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            CliMain();
        }
    }

    private void CliWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        CommandBox.Focusable = true;
        CommandBox.Focus();
    }
}