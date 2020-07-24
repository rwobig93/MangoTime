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
            await Log("Starting bot", LogSeverity.Info);

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
                switch (msg.Severity)
                {
                    case LogSeverity.Critical:
                    case LogSeverity.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogSeverity.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogSeverity.Info:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogSeverity.Verbose:
                    case LogSeverity.Debug:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                }
                Console.WriteLine($"{DateTime.Now,-19} [{msg.Severity,8}] {msg.Source}: {msg.Message} {msg.Exception}");
                Console.ResetColor();

                //Serilog.Log.Logger.Information(msg.ToString());
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now,-19} [{msg.Severity,8}] EXCEPTION: {ex.Message}");
                Console.ResetColor();
            }
            return Task.CompletedTask;
        }

        public static Task Log(string msg, LogSeverity logSeverity)
        {
            try
            {
                switch (logSeverity)
                {
                    case LogSeverity.Critical:
                    case LogSeverity.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogSeverity.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogSeverity.Info:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogSeverity.Verbose:
                    case LogSeverity.Debug:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                }
                Console.WriteLine($"{DateTime.Now,-19} [{logSeverity,8}] ManualLog: {msg}");
                Console.ResetColor();

                //Serilog.Log.Logger.Information(msg);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now,-19} [{logSeverity,8}] EXCEPTION: {ex.Message}");
                Console.ResetColor();
            }
            return Task.CompletedTask;
        }
    }
}
