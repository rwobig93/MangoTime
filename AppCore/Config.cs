using MangoTime.AppCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime
{
    public class Config
    {
        public List<MangoAppearance> MangoAppearances { get; set; } = new List<MangoAppearance>();
        public string BotToken { get; set; }
        public string DirConfig { get; set; } = WTFile.GetConfigPath();
        public string ConfigFilePath { get; set; }
        public DateTime ExpectedTime { get { return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 00, 00); } }

        internal static bool LoadConfig()
        {
            // Validate current config path exists, otherwise set the default
            if (string.IsNullOrWhiteSpace(Program.Config.ConfigFilePath))
            {
                Program.Config.ConfigFilePath = $@"{Program.Config.DirConfig}\config.json";
            }
            if (!WTFile.DirectoryExists(Program.Config.DirConfig))
            {
                Events.Log("Creating non-existant config directory");
                Directory.CreateDirectory(Program.Config.DirConfig);
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
                Events.Log(ex.Message, Discord.LogSeverity.Critical);
            }
            return true;
        }

        internal static void SaveConfig()
        {
            try
            {
                File.WriteAllText(Program.Config.ConfigFilePath, JsonConvert.SerializeObject(Program.Config));
                Events.Log("Config saved successfully");
            }
            catch (Exception ex)
            {
                Events.Log($"Config Save Failure: {ex.Message}", Discord.LogSeverity.Critical);
            }
        }
    }
}
