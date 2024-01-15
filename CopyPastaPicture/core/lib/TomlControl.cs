using System;
using System.IO;
using Tommy;

namespace CopyPastaPicture.core.lib;

public class TomlControl
{
    private LogController _logController = new();
    public struct TomlMain
    {
        public const string TomlDataDir = "./Data/Setting.toml";
        public const string CreateDir = "Data";
        public TomlMain(){}
    }
    public void Initialize()
    {
        if (File.Exists("./Data/Setting.toml")) return;
        _logController.Initialize();
        try
        {
            Directory.CreateDirectory("Data");

            TomlTable toml = new TomlTable
            {
                ["Lang"] = "en-US",
                ["WindowResolution"] =
                {
                    ["Width"] = 800,
                    ["Height"] = 400
                },
                ["CliKey"] = "Ctrl+Shift+P",
            };

            using (StreamWriter writer = File.CreateText("./Data/Setting.toml"))
            {
                toml.WriteTo(writer);
                writer.Flush();
            }
            _logController.InfoLog("Initialize Toml");
        }
        catch (Exception e)
        {
            _logController.ErrorLog($"Initialize Toml Error : {e}");
            Console.WriteLine(e);
            throw;
        }
    }

    public string LanguageName()
    {
        try
        {
            using (StreamReader reader = File.OpenText(TomlControl.TomlMain.TomlDataDir))
            {
                TomlTable table = TOML.Parse(reader);

                var language = table["Lang"];
                
                _logController.InfoLog("SetWindowResolution Success");
                return language;
            }
        }
        catch (Exception e)
        {
            _logController.ErrorLog($"SetWindowResolution Error : {e}");
            Console.WriteLine(e);
            throw;
        }
    }
}