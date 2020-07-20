using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime
{
    public class Config
    {
        public string BotToken { get; set; }
        public string DirConfig { get; set; } = WTFile.GetConfigPath();
        public string ConfigFilePath { get; set; }

        internal static bool LoadConfig()
        {
            // Validate config directory and file exists
            if (!WTFile.DirectoryExists(Program.Config.DirConfig))
            {
                Directory.CreateDirectory(Program.Config.DirConfig);
                Program.Config.ConfigFilePath = $@"{Program.Config.DirConfig}\config.json";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Program.Config.ConfigFilePath))
            {
                Program.Config.ConfigFilePath = $@"{Program.Config.DirConfig}\config.json";
            }
            if (!WTFile.FileExists(Program.Config.ConfigFilePath))
            {
                return false;
            }

            // Import config if config file exists
            try
            {
                var configLoaded = File.ReadAllText(Program.Config.ConfigFilePath);
                Program.Config = JsonConvert.DeserializeObject<Config>(configLoaded);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
            return true;
        }

        internal static void SaveConfig()
        {
            try
            {
                File.WriteAllText(Program.Config.ConfigFilePath, JsonConvert.SerializeObject(Program.Config));
                Console.WriteLine("Config saved successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Config Save Failure: {ex.Message}");
                Console.Read();
            }
        }
    }
}
