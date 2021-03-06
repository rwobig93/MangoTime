﻿using Discord;
using Discord.WebSocket;
using Hangfire;
using Hangfire.MemoryStorage;
using MangoTime.AppCore;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using static MangoTime.Events;

namespace MangoTime
{
    public static class MT
    {
        public static LoggingLevelSwitch LevelSwitch { get; private set; } = new LoggingLevelSwitch();
        public static Logger Logger { get; private set; }

        internal static void ParseLaunchArgs(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.ToLower() == "-debug")
                {
                    // TO-DO: Set log level
                }
            }
        }

        public static void InitializeLogger()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.File($"{WTFile.GetLoggingPath()}\\MangoTime.log", rollingInterval: RollingInterval.Day)
                .MinimumLevel.ControlledBy(LevelSwitch)
                .CreateLogger();
#if DEBUG
            LevelSwitch.MinimumLevel = LogEventLevel.Debug;
#else
            LevelSwitch.MinimumLevel = LogEventLevel.Information;
#endif
            Logger.Information("Logger started");
        }
        internal static UserStatus? GetUserStatusAsync(string usernameContains)
        {
            // Grab Mango user
            var user = GetUserContaining(usernameContains);

            // Return Status or null if user wasn't found
            if (null == user)
            {
                return null;
            }
            else
            {
                return user.Status;
            }
        }

        internal static bool UserHasDesktopLogin(string usernameContains)
        {
            // Grab Mango user
            var user = GetUserContaining(usernameContains);

            // Return Status or null if user wasn't found
            if (null == user)
            {
                return false;
            }
            else
            {
                // If user doesn't have any active clients return false
                if (user.ActiveClients.Count <= 0)
                    return false;
                // If user has 1 or more desktop clients return true
                else if (user.ActiveClients.ToList().FindAll(x => x == ClientType.Desktop).Count >= 1)
                {
                    return true;
                }
                // If user doesn't have 1 or more desktop clients return false
                else
                {
                    return false;
                }
            }
        }

        internal static bool UserHasMobileLogin(string usernameContains)
        {
            // Grab Mango user
            var user = GetUserContaining(usernameContains);

            // Return Status or null if user wasn't found
            if (null == user)
            {
                return false;
            }
            else
            {
                // If user doesn't have any active clients return false
                if (user.ActiveClients.Count <= 0)
                    return false;
                // If user has 1 or more Mobile clients return true
                else if (user.ActiveClients.ToList().FindAll(x => x == ClientType.Mobile).Count >= 1)
                {
                    return true;
                }
                // If user doesn't have 1 or more Mobile clients return false
                else
                {
                    return false;
                }
            }
        }

        internal static bool UserHasWebLogin(string usernameContains)
        {
            // Grab Mango user
            var user = GetUserContaining(usernameContains);

            // Return Status or null if user wasn't found
            if (null == user)
            {
                return false;
            }
            else
            {
                // If user doesn't have any active clients return false
                if (user.ActiveClients.Count <= 0)
                    return false;
                // If user has 1 or more Web clients return true
                else if (user.ActiveClients.ToList().FindAll(x => x == ClientType.Web).Count >= 1)
                {
                    return true;
                }
                // If user doesn't have 1 or more Web clients return false
                else
                {
                    return false;
                }
            }
        }

        internal static void AddMangoAppearance()
        {
            Log("Starting AddMangoAppearance()", LogSeverity.Debug);
            var mangoAppearance = new MangoAppearance
            {
                AppearanceTime = DateTime.Now.ToLocalTime(),
                TimeOffset = DateTime.Now.ToLocalTime() - Program.Config.ExpectedTime,
                ScheduledTime = Program.Config.ExpectedTime
            };
            Program.Config.MangoAppearances.Add(mangoAppearance);
            Log($"Added MangoAppearance: [{mangoAppearance.AppearanceTime}] [{mangoAppearance.ScheduledTime}] [{mangoAppearance.TimeOffset}]");
        }

        internal static void AddMangoAppearanceTest()
        {
            Log("Starting AddMangoAppearanceTest()", LogSeverity.Debug);
            for (int i = 1; i < 6; i++)
            {
                var mangoAppearance = new MangoAppearance
                {
                    AppearanceTime = DateTime.Now.ToLocalTime().AddHours(-i),
                    TimeOffset = DateTime.Now.ToLocalTime().AddHours(-i) - Program.Config.ExpectedTime,
                    ScheduledTime = Program.Config.ExpectedTime
                };
                Program.Config.MangoAppearances.Add(mangoAppearance);
                Log($"Added Test MangoAppearance: [{mangoAppearance.AppearanceTime}] [{mangoAppearance.ScheduledTime}] [{mangoAppearance.TimeOffset}]", LogSeverity.Warning);
            }
        }

        internal static async void ValidateReqs()
        {
            if (string.IsNullOrWhiteSpace(Program.Config.BotToken))
            {
                UI.AskForBotToken();
                Config.SaveConfig();
            }
            else if (!await BotClient.VerifyToken(Program.Config.BotToken))
            {
                Console.WriteLine("Saved token is invalid, please enter a new one!");
                UI.AskForBotToken();
                Config.SaveConfig();
            }
            else
            {
                Console.WriteLine("Token valid, moving on");
            }
            ValidateMangoAppearanceMinimum();
        }

        private static void ValidateMangoAppearanceMinimum()
        {
            if (Program.Config.MangoAppearances.Count <= 0)
            {
                Program.Config.MangoAppearances.Add(new MangoAppearance
                {
                    AppearanceTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 30, 00),
                    RecordNumber = 1,
                    TimeOffset = new TimeSpan(2, 30, 00)
                });
            }
        }

        internal static void StartTimedJobs()
        {
            Log("Starting StartTimedJobs()", LogSeverity.Debug);
            //InitializeHangFire();
            //RecurringJob.AddOrUpdate("MangoCheck", () => Jobs.CheckOnMango(), "*/1 * * * *");
            ThreadMinuteManual();
            Log("Finished Initializing Timed Jobs");
        }

        private static void ThreadMinuteManual()
        {
            BackgroundWorker worker = new BackgroundWorker() { WorkerReportsProgress = true };
            worker.DoWork += (ws, we) =>
            {
                while (true)
                {
                    try
                    {
                        // Sleep on startup to prevent exception for Bot not having full enumeration
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                        Jobs.CheckOnMango();
                    }
                    catch (Exception ex)
                    {
                        Log($"EXCEPTION: {ex.Message}{Environment.NewLine}{ex.StackTrace}", LogSeverity.Critical);
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
            };
            worker.RunWorkerAsync();
        }

        private static void InitializeHangFire()
        {
            Log("Starting InitializeHangFire()");
            GlobalConfiguration.Configuration.UseMemoryStorage();
            Log("Hangfire successfully initialized");
        }

        internal static SocketGuildUser GetUserContaining(string nameContains)
        {
            // Return SocketUser where channel name and username contains provided strings
            return Program.Bot.Client.Guilds.FirstOrDefault().DefaultChannel.
                Users.FirstOrDefault(x => x.Username.ToLower().Contains(nameContains));
        }

        internal static SocketGuildUser GetUserContaining(string channelPrimaryContains, string nameContains)
        {
            // Return SocketUser where channel name and username contains provided strings
            return Program.Bot.Client.Guilds.FirstOrDefault().Channels.
                FirstOrDefault().
                Users.FirstOrDefault(x => x.Username.ToLower().Contains(nameContains));
        }

        internal static string CalculateMangoTime()
        {
            var timeOffset = Program.Config.MangoAppearances.OrderByDescending(x => x.AppearanceTime).First().TimeOffset;

            DateTime mangoTime = DateTime.Now.ToLocalTime().AddHours(timeOffset.TotalHours);

            string mangoString = $"Current MangoTime is: {mangoTime.ToShortTimeString()}{Environment.NewLine}(Offset: {timeOffset.Hours}h {timeOffset.Minutes}m {timeOffset.Seconds}s {timeOffset.Milliseconds}ms {timeOffset.Ticks}ticks)";

            return mangoString;
        }
    }
}
