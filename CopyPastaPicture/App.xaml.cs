using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CopyPastaPicture.core;
using CopyPastaPicture.core.lang;
using CopyPastaPicture.core.lib;
using CopyPastaPicture.core.window;
using Application = System.Windows.Application;

namespace CopyPastaPicture
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.ContextMenuStrip _menu = new();
        private System.Windows.Forms.NotifyIcon _notifyIcon = new();
        private TomlControl _tomlControl = new();
        private LogController _logController = new();
        protected override void OnStartup(StartupEventArgs e)
        {
            // ./core/lib/LogController.cs のInitialize
            _logController.Initialize();
            // ./core/lib/TomlControl.cs のInitialize
            _tomlControl.Initialize();

            switch (_tomlControl.LanguageName())
            {
                case "en-US":
                    //タクストレイのコード
                    _menu.Items.Add(EnLanguage.Open, null, (obj, e) => { new MainWindow().Show(); });
                    _menu.Items.Add(EnLanguage.Setting, null, (obj, e) => { new SettingWindow().Show(); });
                    _menu.Items.Add(EnLanguage.Exit, null, (obj, e) => { Application.Current.Shutdown(); });
                    break;
                case "ja-JP":
                    
                    //タクストレイのコード
                    _menu.Items.Add(JaLanguage.Open, null, (obj, e) => { new MainWindow().Show(); });
                    _menu.Items.Add(JaLanguage.Setting, null, (obj, e) => { new SettingWindow().Show(); });
                    _menu.Items.Add(JaLanguage.Exit, null, (obj, e) => { Application.Current.Shutdown(); });
                    break;
            }
    
            _notifyIcon.Visible = true;
            _notifyIcon.Icon = new System.Drawing.Icon("./resources/ico/Pastaicon.ico");
            _notifyIcon.Text = "CopyPastaPicture";
            _notifyIcon.ContextMenuStrip = _menu;
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _menu.Dispose();
            _logController.InfoLog("Menu Dispose Success");
            _notifyIcon.Dispose();
            _logController.InfoLog("NotifyIcon Dispose Success");
            
            base.OnExit(e);
        }
    }
}