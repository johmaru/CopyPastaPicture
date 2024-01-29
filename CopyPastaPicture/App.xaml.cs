using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using CopyPastaPicture.core;
using CopyPastaPicture.core.lang;
using CopyPastaPicture.core.lib;
using CopyPastaPicture.core.page;
using CopyPastaPicture.core.window;
using Application = System.Windows.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using KeyEventHandler = System.Windows.Input.KeyEventHandler;

namespace CopyPastaPicture
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    
    public partial class App : Application
    {
        private System.Windows.Forms.ContextMenuStrip _menu = new();
        public MainPage MainPageInstance { get; set; }
        private MainWindow MainWindowInstance { get; set; }
        private System.Windows.Forms.NotifyIcon _notifyIcon = new();
        private TomlControl _tomlControl = new();
        private LogController _logController = new();
        private const int Opkey = 0x0001;
        private const int Clikey = 0x0002;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            // ./core/lib/LogController.cs のInitialize
            _logController.Initialize();
            // ./core/lib/TomlControl.cs のInitialize
            _tomlControl.Initialize();
            
            LoadNotifyIcon();
            
            EventManager.RegisterClassHandler(typeof(Window), Window.KeyDownEvent, new KeyEventHandler(OnKeyDown), true);
            
            base.OnStartup(e);
            MainPageInstance = new MainPage();
            MainWindowInstance = new MainWindow();
        }
        
        public void LoadNotifyIcon()
        {
            _menu.Items.Clear();
            switch (_tomlControl.LanguageName())
            {
                case "en-US":
                    //タクストレイのコード
                    _menu.Items.Add(EnLanguage.Open, null, (obj, e) => { new MainWindow().Show(); });
                    _menu.Items.Add(EnLanguage.OpenCliButton, null, (obj, e) => { new CliWindow().Show(); });
                    _menu.Items.Add(EnLanguage.Setting, null, (obj, e) => { new SettingWindow().Show(); });
                    _menu.Items.Add(EnLanguage.Exit, null, (obj, e) => { Application.Current.Shutdown(); });
                    break;
                case "ja-JP":
                    
                    //タクストレイのコード
                    _menu.Items.Add(JaLanguage.Open, null, (obj, e) => { new MainWindow().Show(); });
                    _menu.Items.Add(JaLanguage.OpenCliButton, null, (obj, e) => { new CliWindow().Show(); });
                    _menu.Items.Add(JaLanguage.Setting, null, (obj, e) => { new SettingWindow().Show(); });
                    _menu.Items.Add(JaLanguage.Exit, null, (obj, e) => { Application.Current.Shutdown(); });
                    break;
            }
    
            _notifyIcon.Visible = true;
            _notifyIcon.Icon = new System.Drawing.Icon("./resources/ico/Pastaicon.ico");
            _notifyIcon.Text = "CopyPastaPicture";
            _notifyIcon.MouseUp += (obj, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Current.Windows.OfType<MainWindow>().Any(x => x.IsActive))return;
                    MainWindowInstance.Show();
                }
            };
            _notifyIcon.ContextMenuStrip = _menu;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _menu.Dispose();
            _logController.InfoLog("Menu Dispose Success");
            _notifyIcon.Dispose();
            _logController.InfoLog("NotifyIcon Dispose Success");
            
            base.OnExit(e);
        }
        
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.P))
            {
                if (Application.Current.Windows.OfType<CliWindow>().Any())
                {
                    var searchWindow = Application.Current.Windows.OfType<CliWindow>().First();
                    searchWindow.Close();
                }
                else if (!Application.Current.Windows.OfType<CliWindow>().Any())
                {
                    CliWindow cliWindow = new();
                    cliWindow.Show();
                }
                else
                {
                    _logController.ErrorLog("Unknown Error : panic in CliWindow");
                    this.Shutdown();
                }
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl)  && Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.O))
            {
                if (Application.Current.Windows.OfType<MainWindow>().Any())
                {
                    var searchWindow = Application.Current.Windows.OfType<MainWindow>().First();
                    searchWindow.Close();
                }
                else if (!Application.Current.Windows.OfType<MainWindow>().Any())
                {
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                }
                else
                {
                    _logController.ErrorLog("Unknown Error : panic in MainWindow");
                    this.Shutdown();
                }
            }
            {
                
            }
        }
    }
}