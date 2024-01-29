using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CopyPastaPicture.core.lang;
using CopyPastaPicture.core.lib;
using CopyPastaPicture.core.window;
using ModernWpf.Controls;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using MessageBox = System.Windows.MessageBox;
using Orientation = System.Windows.Controls.Orientation;
using Page = System.Windows.Controls.Page;
using TextBox = System.Windows.Controls.TextBox;

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

    public void InitializeContent()
    {
        FileSearch();
    }

    private async Task ShowContentDialogAsync(string dir)
    {
        switch (_tomlControl.LanguageName())
        {
            case "en-US":
                ContentDialog contentDialog = new ContentDialog
                {
                    Title = EnLanguage.CreateImport,
                    Content = EnLanguage.CreateImport,
                    PrimaryButtonText = EnLanguage.CreateFolder,
                    SecondaryButtonText = EnLanguage.ImportPicture,
                    CloseButtonText = EnLanguage.cancel
                };
                ContentDialogResult  result = await contentDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                            Window window = new Window();
                            window.Height = 100;
                            window.Width = 256;
                             Grid grid = new Grid();
                            TextBox textBox = new TextBox();
                            textBox.Height = 32;
                            textBox.Width = 256;
                            textBox.Text = EnLanguage.SimpleInputName;
                            textBox.KeyUp += (obj, e) =>
                            {
                                if (e.Key == System.Windows.Input.Key.Enter)
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(dir + "/" + textBox.Text);
                                        _logController.InfoLog($"Create Directory Success {dir + "/" + textBox.Text}");
                                        MessageBox.Show(EnLanguage.CreateDir, EnLanguage.CreateDir, MessageBoxButton.OK);
                                        ((App)Application.Current).ResetMainPage();
                                        window.Close();
                                    }
                                    catch (Exception exception)
                                    {
                                        _logController.ErrorLog($"Create Directory Error {exception}");
                                        throw;
                                    }
                                }
                            };
                            grid.Children.Add(textBox);
                           window.Content = grid;
                           window.Show();
                }
                else if (result == ContentDialogResult.Secondary)
                {
                            Window window = new Window();
                            Grid grid1 = new Grid();
                            TextBox nameBox2 = new TextBox();
                            window.Height = 100;
                            window.Width = 256;
                            nameBox2.Height = 32;
                            nameBox2.Width = 256;
                            nameBox2.Text = EnLanguage.SimpleInputName;
                            nameBox2.KeyUp += (obj, e) =>
                            {
                                if (e.Key == System.Windows.Input.Key.Enter)
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(dir + "/" + nameBox2.Text);
                                        using (var file = new OpenFileDialog())
                                        {
                                    
                                            file.Title = EnLanguage.PictureSelect;
                                            file.FileName = EnLanguage.ExamplePictureName;
                                            file.Filter = "Image File(*.png, *.jpg, *.jpeg, *.gif)|*.png;*.jpg;*.jpeg;*.gif";
                                            file.CheckFileExists = false;
                                            file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                                            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                            {
                                                try
                                                {
                                                    var fi = new FileInfo(file.FileName);
                                                    string path = file.SafeFileName;
                                            
                                                    Directory.CreateDirectory($"./Data/Image/{nameBox2.Text}");
                                                    if (File.Exists($"./Data/Image/{nameBox2.Text}")) return;
                                                    fi.CopyTo(($"./Data/Image/{nameBox2.Text}/{path}"));
                                                    _logController.InfoLog($"File Move Success to ./Data/Image/{path}/{nameBox2.Text}");
                                                    ((App)Application.Current).ResetMainPage();
                                                    window.Close();
                                                }
                                                catch (Exception exception)
                                                {
                                                    _logController.ErrorLog($"File Move Error{exception}");
                                                    throw;
                                                } 
                                            }
                                        } 
                                        _logController.InfoLog($"Create Directory Success {dir + "/" + nameBox2.Text}");
                                        MessageBox.Show(EnLanguage.ImportPicture, EnLanguage.ImportPicture, MessageBoxButton.OK);
                                    }
                                    catch (Exception exception)
                                    {
                                        _logController.ErrorLog($"Create Directory Error {exception}");
                                        throw;
                                    }
                                }
                            };
                    grid1.Children.Add(nameBox2);
                    window.Content = grid1;
                    window.Show();
                }
                else
                {
                    Console.WriteLine("Cancel");
                }
                break;
            case "ja-JP":
                    ContentDialog contentDialog1 = new ContentDialog
                    {
                        Title = JaLanguage.CreateImport,
                        Content = JaLanguage.CreateImport,
                        PrimaryButtonText = JaLanguage.CreateFolder,
                        SecondaryButtonText = JaLanguage.ImportPicture,
                        CloseButtonText = JaLanguage.cancel
                    };
                    ContentDialogResult  result1 = await contentDialog1.ShowAsync();
                if (result1 == ContentDialogResult.Primary)
                {
                    Window window = new Window();
                    Grid grid2 = new Grid();
                    TextBox nameBox = new TextBox();
                    window.Height = 100;
                    window.Width = 256;
                    nameBox.Height = 32;
                    nameBox.Width = 256;
                    nameBox.Text = JaLanguage.SimpleInputName;
                    nameBox.KeyUp += (obj, e) =>
                    {
                        if (e.Key == System.Windows.Input.Key.Enter)
                        {
                            try
                            {
                                Directory.CreateDirectory(dir + "/" + nameBox.Text);
                                _logController.InfoLog($"Create Directory Success {dir + "/" + nameBox.Text}");
                                MessageBox.Show(JaLanguage.CreateDir, JaLanguage.CreateDir, MessageBoxButton.OK);
                                ((App)Application.Current).ResetMainPage();
                                window.Close();
                            }
                            catch (Exception exception)
                            {
                                _logController.ErrorLog($"Create Directory Error {exception}");
                                throw;
                            }
                        }
                    };
                    grid2.Children.Add(nameBox);
                    window.Content = grid2;
                    window.Show();
                }
                else if (result1 == ContentDialogResult.Secondary)
                {
                    Window window = new Window();
                    Grid grid3 = new Grid();
                    window.Height = 100;
                    window.Width = 256;
                     TextBox nameBox3 = new TextBox
                     {
                         Height = 32,
                         Width = 256,
                         Text = JaLanguage.SimpleInputName
                     };
                     nameBox3.KeyUp += (obj, e) =>
                            {
                                if (e.Key == System.Windows.Input.Key.Enter)
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(dir + "/" + nameBox3.Text);
                                        using (var file = new OpenFileDialog())
                                        {
                                    
                                            file.Title = JaLanguage.PictureSelect;
                                            file.FileName = JaLanguage.ExamplePictureName;
                                            file.Filter = "Image File(*.png, *.jpg, *.jpeg, *.gif)|*.png;*.jpg;*.jpeg;*.gif";
                                            file.CheckFileExists = false;
                                            file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                                            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                            {
                                                try
                                                {
                                                    var fi = new FileInfo(file.FileName);
                                                    string path = file.SafeFileName;
                                            
                                                    Directory.CreateDirectory($"./Data/Image/{nameBox3.Text}");
                                                    if (File.Exists($"./Data/Image/{nameBox3.Text}")) return;
                                                    fi.CopyTo(($"./Data/Image/{nameBox3.Text}/{path}"));
                                                    _logController.InfoLog($"File Move Success to ./Data/Image/{path}/{nameBox3.Text}");
                                                    ((App)Application.Current).ResetMainPage();
                                                    window.Close();
                                                }
                                                catch (Exception exception)
                                                {
                                                    _logController.ErrorLog($"File Move Error{exception}");
                                                    throw;
                                                } 
                                            }
                                        } 
                                        _logController.InfoLog($"Create Directory Success {dir + "/" + nameBox3.Text}");
                                        MessageBox.Show(JaLanguage.ImportPicture, JaLanguage.ImportPicture, MessageBoxButton.OK);
                                    }
                                    catch (Exception exception)
                                    {
                                        _logController.ErrorLog($"Create Directory Error {exception}");
                                        throw;
                                    }
                                }
                            };
                            grid3.Children.Add(nameBox3);
                            window.Content = grid3;
                            window.Show();
                }
                else
                {
                    Console.WriteLine("Cancel");
                }
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
                        var createImportItem = new MenuItem { Header = EnLanguage.CreateImport };
                        var deleteItem = new MenuItem { Header = EnLanguage.SimpleDelete };
                        var openItem = new MenuItem { Header = EnLanguage.SimpleOpen };
                        var copyItem = new MenuItem { Header = EnLanguage.SimpleCopy };

                        createImportItem.Click += async (obj, e) =>
                        {
                            try
                            {
                                        await ShowContentDialogAsync(Path.GetFullPath(path));
                                        _logController.InfoLog($"File or Directory Import Success {Path.GetFullPath(path)}");
                            }
                            catch (Exception exception)
                            {
                                _logController.ErrorLog($"File or Directory Import Error{exception}");
                                throw;
                            }
                        };
                        
                        deleteItem.Click += (obj, e) =>
                        {
                            try
                            {
                                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                                {
                                    Directory.Delete(Path.GetFullPath(path), true);
                                    _logController.InfoLog($"File or Directory Delete Success {Path.GetFullPath(path)}");
                                    ((App)Application.Current).ResetMainPage();
                                }
                                else if (File.GetAttributes(path).HasFlag(FileAttributes.Normal))
                                {
                                    File.Delete(Path.GetFullPath(path));
                                    _logController.InfoLog($"File or Directory Delete Success {Path.GetFullPath(path)}");
                                    ((App)Application.Current).ResetMainPage();
                                }
                                else
                                {
                                 _logController.ErrorLog($"File or Directory Delete Error");   
                                }
                            }
                            catch (Exception exception)
                            {
                                _logController.ErrorLog($"File or Directory Delete Error{exception}");
                                throw;
                            }
                        };
                        
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
                        contextMenu.Items.Add(createImportItem);
                        contextMenu.Items.Add(deleteItem);
                        contextMenu.Items.Add(openItem);
                        contextMenu.Items.Add(copyItem);
                        newItem.ContextMenu = contextMenu;
                    }
                    break;
                case "ja-JP":
                    if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
                    {
                        var contextMenu = new ContextMenu();
                        var createImportItem = new MenuItem { Header = EnLanguage.CreateImport };
                        var deleteItem = new MenuItem { Header = JaLanguage.SimpleDelete };
                        var openItem = new MenuItem { Header = JaLanguage.SimpleOpen };
                        var copyItem = new MenuItem { Header = JaLanguage.SimpleCopy };

                        createImportItem.Click += async (obj, e) =>
                        {
                          await ShowContentDialogAsync(Path.GetFullPath(path));
                        };
                        
                        deleteItem.Click += (obj, e) =>
                        {
                            try
                            {
                                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                                {
                                    Directory.Delete(Path.GetFullPath(path), true);
                                    _logController.InfoLog($"File or Directory Delete Success {Path.GetFullPath(path)}");
                                    ((App)Application.Current).ResetMainPage();
                                }
                                else if (File.GetAttributes(path).HasFlag(FileAttributes.Normal))
                                {
                                    File.Delete(Path.GetFullPath(path));
                                    _logController.InfoLog($"File or Directory Delete Success {Path.GetFullPath(path)}");
                                    ((App)Application.Current).ResetMainPage();
                                }
                                else
                                {
                                 _logController.ErrorLog($"File or Directory Delete Error");   
                                }
                            }
                            catch (Exception exception)
                            {
                                _logController.ErrorLog($"File or Directory Delete Error{exception}");
                                throw;
                            }
                        };

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
                        contextMenu.Items.Add(createImportItem);
                        contextMenu.Items.Add(deleteItem);
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

    private void FileView_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        //スクロールバーの移動
    }
}