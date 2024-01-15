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
                ["Theme"] = "Dark",
                ["WindowResolution"] =
                {
                    ["Width"] = 800,
                    ["Height"] = 400
                }
            };

            using (StreamWriter writer = File.CreateText("./Data/Setting.toml"))
            {
                toml.WriteTo(writer);
                writer.Flush();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        _logController.InfoLog("Initialize Toml");
    }
}