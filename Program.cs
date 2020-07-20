using Discord;
using System;
using System.Threading.Tasks;

namespace MangoTime
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public static Config Config { get; set; } = new Config();

        public async Task MainAsync()
        {
            // Log startup
            await Log("Starting bot");

            // Catch App Close and try to finish up, default timer is 3 seconds
            AppDomain.CurrentDomain.ProcessExit += Events.ConsoleClose;

            // Load Config and Validate Requirements
            Config.LoadConfig();
            MT.ValidateReqs();

            // Start bot
            await BotClient.StartBot();

            // Block task until closed
            await Task.Delay(-1);
        }

        public static Task Log(LogMessage msg)
        {
            try
            {
                //Serilog.Log.Logger.Information(msg.ToString());
                Console.WriteLine(msg.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
            }
            return Task.CompletedTask;
        }

        public static Task Log(string msg)
        {
            try
            {
                //Serilog.Log.Logger.Information(msg);
                Console.WriteLine(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
            }
            return Task.CompletedTask;
        }
    }
}
