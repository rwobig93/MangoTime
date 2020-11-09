using Discord;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MangoTime
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public static Config Config { get; set; } = new Config();
        public static BotClient Bot { get; set; }

        public async Task MainAsync(string[] args)
        {
            // Initialize Logger
            MT.InitializeLogger();

            // Validate launch args
            MT.ParseLaunchArgs(args);

            // Log startup
            await Events.Log("Starting bot", LogSeverity.Info);

            // Catch App Close and try to finish up, default timer is 3 seconds
            AppDomain.CurrentDomain.ProcessExit += Events.ConsoleClose;

            // Load Config and Validate Requirements
            Config.LoadConfig();
            MT.ValidateReqs();

            // Initialize and Start bot
            Bot = new BotClient();
            await Bot.StartBot();

            // Start timed jobs
            MT.StartTimedJobs();

            // Block task until closed
            await Task.Delay(Timeout.Infinite);
        }
    }
}
