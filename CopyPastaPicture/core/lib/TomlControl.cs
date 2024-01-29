using System;
using System.Collections;
using System.IO;
using Tommy;

namespace CopyPastaPicture.core.lib;

public class TomlControl
{
    public bool IsString { get; } = true;
    public bool IsBool { get; } = true;
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
                ["CliMode"] = "true",
                ["WindowResolution"] =
                {
                    ["Width"] = 800,
                    ["Height"] = 400
                },
                ["CliKey"] = "Ctrl+Shift+P",
                ["CliWindowResolution"] =
                {
                    ["Width"] = 400,
                    ["Height"] = 100
                }
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

    public string GetTomlData(string tomlString)
    {
        try
        {
            using (StreamReader reader = File.OpenText(TomlControl.TomlMain.TomlDataDir))
            {
                TomlTable table = TOML.Parse(reader);

                var language = table[tomlString];
                
                _logController.InfoLog("GetTomlData Success");
                return language;
            }
        }
        catch (Exception e)
        {
            _logController.ErrorLog($"GetTomlData Error : {e}");
            Console.WriteLine(e);
            throw;
        }
    }
}