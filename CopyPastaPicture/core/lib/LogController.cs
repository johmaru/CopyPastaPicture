using System;
using NLog;

namespace CopyPastaPicture.core.lib;

public class LogController
{
    public void Initialize()
    {
        var config = new NLog.Config.LoggingConfiguration();

        var date = DateTime.Now.ToString("O");
        
        var logfile = new NLog.Targets.FileTarget("logfile"){FileName = $"./Log/Log-{date}.txt"};
        
        config.AddRuleForAllLevels(logfile);

        LogManager.Configuration = config;
    }
    
    public void InfoLog(string message)
    {
        LogManager.GetCurrentClassLogger().Info(message);
    }
  
    public void ErrorLog(string message)
    {
        LogManager.GetCurrentClassLogger().Error(message);
    }
    
    public void DebugLog(string message)
    {
        LogManager.GetCurrentClassLogger().Debug(message);
    }
    
    public void FatalLog(string message)
    {
        LogManager.GetCurrentClassLogger().Fatal(message);
    }
    
    public void WarnLog(string message)
    {
        LogManager.GetCurrentClassLogger().Warn(message);
    }
    
    public void TraceLog(string message)
    {
        LogManager.GetCurrentClassLogger().Trace(message);
    }
    
}