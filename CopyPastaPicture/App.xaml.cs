using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CopyPastaPicture.core;
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
        private LogController _logController = new();
        protected override void OnStartup(StartupEventArgs e)
        {
            // ./core/lib/LogController.cs のInitialize
            _logController.Initialize();
            //タクストレイのコード
            _menu.Items.Add("Open CopyPastaPicture", null, (obj, e) => { new MainWindow().Show(); });
            _menu.Items.Add("Setting", null, (obj, e) => { new SettingWindow().Show(); });
            _menu.Items.Add("Exit", null, (obj, e) => { Application.Current.Shutdown(); });
    
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