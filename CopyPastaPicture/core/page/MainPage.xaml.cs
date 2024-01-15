using System.Windows.Controls;
using CopyPastaPicture.core.lib;

namespace CopyPastaPicture.core.page;

public partial class MainPage : Page
{
   private LogController _logController = new();
    public MainPage()
    {
        InitializeComponent();
        Initialize();
    }

    private void Initialize()
    {
        _logController.InfoLog("Initialize MainPage");
    }
}