using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
        FileSearch();
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
        InitializeContent();
    }

    private void InitializeContent()
    {
        FileSearch();
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

    private void FileSearch()
    {
        try
        {
            if (!Directory.Exists("./Data"))
            {
                Directory.CreateDirectory("./Data");
            }
            
            FileView.Items.Clear();

            var dataDirectories = Directory.GetDirectories("Data");
            foreach (var directory in dataDirectories)
            {
                var newItem = CreateTreeViewItem(directory);
                FileView.Items.Add(newItem);
            }
        }
        catch (Exception e)
        {
            _logController.ErrorLog($"FileSearch Error{e}");
            throw;
        }
    }

    private TreeViewItem CreateTreeViewItem(string path)
    {
        //フォルダ画像
        var stackPanel1 = new StackPanel { Orientation = Orientation.Horizontal };
        var folderImage = new Image
        {
            Source = new BitmapImage(new Uri("/resources/ico/File_Explorer_23583.ico", UriKind.Relative)),
            Width = 16,
            Height = 16
        };
        var textBlock = new TextBlock { Text = Path.GetFileName(path), Margin = new Thickness(10, 0, 0, 0) };
        stackPanel1.Children.Add(folderImage);
        stackPanel1.Children.Add(textBlock);
        var newItem = new TreeViewItem { Header = Path.GetFileName(path) };
        newItem.Loaded += (obj, e) =>
        {
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
               newItem.Header = stackPanel1;
            }
            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                var subItem = CreateTreeViewItem(directory);
                newItem.Items.Add(subItem);
            }
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                //ピクチャ画像
                var stackPanel2 = new StackPanel() { Orientation = Orientation.Horizontal };
                var pictureImage = new Image
                {
                    Source = new BitmapImage(new Uri("/resources/ico/picture_photo_image_icon_131252.ico", UriKind.Relative)),
                    Width = 16,
                    Height = 16
                };
                var textBlock2 = new TextBlock { Text = Path.GetFileName(file), Margin = new Thickness(10, 0, 0, 0) };
                stackPanel2.Children.Add(pictureImage);
                stackPanel2.Children.Add(textBlock2);
                var fileItem = new TreeViewItem { Header = Path.GetFileName(file) };
                fileItem.Loaded += (obj, e) =>
                {
                    var pathExtension = Path.GetExtension(Path.GetFullPath(file));
                    Console.WriteLine(pathExtension);
                    if (pathExtension == ".png" || pathExtension == ".jpg" || pathExtension == ".jpeg" || pathExtension == ".gif")
                    {
                        fileItem.Header = stackPanel2;
                    }
                };
                fileItem.MouseRightButtonUp += (obj, e) =>
            {
                switch (_tomlControl.LanguageName())
                {
                    case "en-US" :
                        if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
                        {
                            var contextMenu = new ContextMenu();
                            var openItem = new MenuItem { Header = EnLanguage.SimpleOpen };
                            var copyItem = new MenuItem { Header = EnLanguage.SimpleCopy};
                            openItem.Click += (obj, e) =>
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                    {
                                        FileName = Path.GetFullPath(file),
                                        UseShellExecute = true
                                    });
                                    _logController.InfoLog($"File Open Success {Path.GetFullPath(file)}");
                                }
                                catch (Exception exception)
                                {
                                    _logController.ErrorLog($"File Open Error{exception}");
                                    throw;
                                }
                            };
                            copyItem.Click += (obj, e) =>
                            {
                                try
                                {
                                    if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
                                    {
                                        Clipboard.SetText(Path.GetFullPath(file));
                                    }
                                    else
                                    {
                                        var pathExtension = Path.GetExtension(Path.GetFullPath(file));
                                        Console.WriteLine(pathExtension);
                                        if (pathExtension == ".png" || pathExtension == ".jpg" || pathExtension == ".jpeg" || pathExtension == ".gif")
                                        {
                                            var bitmap = new BitmapImage(new Uri("file://" + Path.GetFullPath(file)));
                                            Clipboard.SetImage(bitmap);
                                        }
                                        else if (pathExtension == ".txt")
                                        {
                                            //Clipboard.SetText(File.ReadAllText(Path.GetFullPath(path)));
                                        }
                                    }
                                    _logController.InfoLog($"File Copy Success {Path.GetFullPath(file)}");
                                }
                                catch (Exception exception)
                                {
                                    _logController.ErrorLog($"File Copy Error{exception}");
                                    throw;
                                }
                            };
                            contextMenu.Items.Add(openItem);
                            contextMenu.Items.Add(copyItem);
                            fileItem.ContextMenu = contextMenu;
                        }
                        break;
                    case "ja-JP":
                        if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
                        {
                            var contextMenu = new ContextMenu();
                            var openItem = new MenuItem { Header = JaLanguage.SimpleOpen };
                            var copyItem = new MenuItem { Header = JaLanguage.SimpleCopy};
                            openItem.Click += (obj, e) =>
                            {
                                try
                                {
                                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                        {
                                            FileName = Path.GetFullPath(file),
                                            UseShellExecute = true
                                        });
                                }
                                catch (Exception exception)
                                {
                                    _logController.ErrorLog($"File Open Error{exception}");
                                    throw;
                                }
                            };
                            copyItem.Click += (obj, e) =>
                            {
                                try
                                {
                                    if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
                                    {
                                        Clipboard.SetText(Path.GetFullPath(file));
                                    }
                                    else
                                    {
                                        var pathExtension = Path.GetExtension(Path.GetFullPath(file));
                                        Console.WriteLine(pathExtension);
                                        if (pathExtension == ".png" || pathExtension == ".jpg" || pathExtension == ".jpeg" || pathExtension == ".gif")
                                        {
                                            var bitmap = new BitmapImage(new Uri(Path.GetFullPath(file)));
                                            Clipboard.SetImage(bitmap);
                                        }
                                        else if (pathExtension == ".txt")
                                        {
                                            //Clipboard.SetText(File.ReadAllText(Path.GetFullPath(path)));
                                        }
                                    }
                                    _logController.InfoLog($"File Copy Success {Path.GetFullPath(file)}");
                                }
                                catch (Exception exception)
                                {
                                    _logController.ErrorLog($"File Copy Error{exception}");
                                    throw;
                                }
                            };
                            contextMenu.Items.Add(openItem);
                            contextMenu.Items.Add(copyItem);
                            fileItem.ContextMenu = contextMenu;
                        }
                        break;
                }
            };
                newItem.Items.Add(fileItem);
            }
        };
        newItem.MouseRightButtonUp += (obj, e) =>
        {
            switch (_tomlControl.LanguageName())
            {
                case "en-US" :
                    if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
                    {
                        var contextMenu = new ContextMenu();
                        var openItem = new MenuItem { Header = EnLanguage.SimpleOpen };
                        var copyItem = new MenuItem { Header = EnLanguage.SimpleCopy };
                        openItem.Click += (obj, e) =>
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = Path.GetFullPath(path),
                                    UseShellExecute = true
                                });
                                _logController.InfoLog($"File Open Success {Path.GetFullPath(path)}");
                            }
                            catch (Exception exception)
                            {
                                _logController.ErrorLog($"File Open Error{exception}");
                                throw;
                            }
                        };
                        copyItem.Click += (obj, e) =>
                        {
                            try
                            {
                                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                                {
                                    Clipboard.SetText(Path.GetFullPath(path));
                                }
                                else
                                {
                                    var pathExtension = Path.GetExtension(Path.GetFullPath(path));
                                    Console.WriteLine(pathExtension);
                                    if (pathExtension == ".png" || pathExtension == ".jpg" || pathExtension == ".jpeg" || pathExtension == ".gif")
                                    {
                                        var bitmap = new BitmapImage(new Uri("file://" + Path.GetFullPath(path)));
                                        Clipboard.SetImage(bitmap);
                                    }
                                    else if (pathExtension == ".txt")
                                    {
                                        //Clipboard.SetText(File.ReadAllText(Path.GetFullPath(path)));
                                    }
                                }
                                _logController.InfoLog($"File Copy Success {Path.GetFullPath(path)}");
                            }
                            catch (Exception exception)
                            {
                                _logController.ErrorLog($"File Copy Error{exception}");
                                throw;
                            }
                        };
                        contextMenu.Items.Add(openItem);
                        contextMenu.Items.Add(copyItem);
                        newItem.ContextMenu = contextMenu;
                    }
                    break;
                case "ja-JP":
                    if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
                    {
                        var contextMenu = new ContextMenu();
                        var openItem = new MenuItem { Header = JaLanguage.SimpleOpen };
                        var copyItem = new MenuItem { Header = JaLanguage.SimpleCopy };
                        openItem.Click += (obj, e) =>
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = Path.GetFullPath(path),
                                    UseShellExecute = true
                                });
                                _logController.InfoLog($"File Open Success {Path.GetFullPath(path)}");
                            }
                            catch (Exception exception)
                            {
                                _logController.ErrorLog($"File Open Error{exception}");
                                throw;
                            }
                        };
                        copyItem.Click += (obj, e) =>
                        {
                            try
                            {
                                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                                {
                                    Clipboard.SetText(Path.GetFullPath(path));
                                }
                                else
                                {
                                    var pathExtension = Path.GetExtension(Path.GetFullPath(path));
                                    Console.WriteLine(pathExtension);
                                    if (pathExtension == ".png" || pathExtension == ".jpg" || pathExtension == ".jpeg" || pathExtension == ".gif")
                                    {
                                        var bitmap = new BitmapImage(new Uri("file://" + Path.GetFullPath(path)));
                                        Clipboard.SetImage(bitmap);
                                    }
                                    else if (pathExtension == ".txt")
                                    {
                                        //Clipboard.SetText(File.ReadAllText(Path.GetFullPath(path)));
                                    }
                                }
                                _logController.InfoLog($"File Copy Success {Path.GetFullPath(path)}");
                            }
                            catch (Exception exception)
                            {
                                _logController.ErrorLog($"File Copy Error{exception}");
                                throw;
                            }
                        };
                        contextMenu.Items.Add(openItem);
                        contextMenu.Items.Add(copyItem);
                        newItem.ContextMenu = contextMenu;
                    }
                    break;
            }
        };
      
        
        return newItem;
        
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