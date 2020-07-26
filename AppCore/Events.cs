using Discord;
using Discord.WebSocket;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime
{
    public static class Events
    {
        public static Task Log(LogMessage msg)
        {
            try
            {
                LogEventLevel serLogLvl = LogEventLevel.Information;
                switch (msg.Severity)
                {
                    case LogSeverity.Critical:
                        serLogLvl = LogEventLevel.Fatal;
                        break;
                    case LogSeverity.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        serLogLvl = LogEventLevel.Error;
                        break;
                    case LogSeverity.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        serLogLvl = LogEventLevel.Warning;
                        break;
                    case LogSeverity.Info:
                        Console.ForegroundColor = ConsoleColor.White;
                        serLogLvl = LogEventLevel.Information;
                        break;
                    case LogSeverity.Verbose:
                        serLogLvl = LogEventLevel.Verbose;
                        break;
                    case LogSeverity.Debug:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        serLogLvl = LogEventLevel.Debug;
                        break;
                }

                MT.Logger.Write(serLogLvl, $"{DateTime.Now,-19} [{msg.Severity,8}] {msg.Source}: {msg.Message} {msg.Exception}");
                Console.WriteLine($"{DateTime.Now,-19} [{msg.Severity,8}] {msg.Source}: {msg.Message} {msg.Exception}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now,-19} [{msg.Severity,8}] EXCEPTION: {ex.Message}");
                Console.ResetColor();
            }
            return Task.CompletedTask;
        }

        public static Task Log(string msg, LogSeverity logSeverity = LogSeverity.Info)
        {
            try
            {
                LogEventLevel serLogLvl = LogEventLevel.Information;
                switch (logSeverity)
                {
                    case LogSeverity.Critical:
                        serLogLvl = LogEventLevel.Fatal;
                        break;
                    case LogSeverity.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        serLogLvl = LogEventLevel.Error;
                        break;
                    case LogSeverity.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        serLogLvl = LogEventLevel.Warning;
                        break;
                    case LogSeverity.Info:
                        Console.ForegroundColor = ConsoleColor.White;
                        serLogLvl = LogEventLevel.Information;
                        break;
                    case LogSeverity.Verbose:
                        serLogLvl = LogEventLevel.Verbose;
                        break;
                    case LogSeverity.Debug:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        serLogLvl = LogEventLevel.Debug;
                        break;
                }

                MT.Logger.Write(serLogLvl, $"{DateTime.Now,-19} [{logSeverity,8}] ManualLog: {msg}");
                Console.WriteLine($"{DateTime.Now,-19} [{logSeverity,8}] ManualLog: {msg}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now,-19} [{logSeverity,8}] EXCEPTION: {ex.Message}");
                Console.ResetColor();
            }
            return Task.CompletedTask;
        }
        internal static Task BotLoggedIn()
        {
            Log("Bot logged in", Discord.LogSeverity.Info);
            return Task.CompletedTask;
        }

        internal static Task BotLoggedOut()
        {
            Log("Bot logged out", Discord.LogSeverity.Info);
            return Task.CompletedTask;
        }

        //internal static async Task MessageReceived(SocketMessage arg)
        //{
        //    if (arg.Content.StartsWith('!'))
        //    {
        //        await Program.Log($"{DateTime.Now.ToLocalTime()} CMD: {arg.Author.Username}: {arg.Content}");
        //        await Commands.HandleMessage(arg);
        //    }
        //    else
        //    {
        //        await Program.Log($"{DateTime.Now.ToLocalTime()} MSG: {arg.Author.Username}: {arg.Content}");
        //    }
        //}

        internal static void ConsoleClose(object sender, EventArgs e)
        {
            Events.Log("Closing!");
            Config.SaveConfig();
            Events.Log("Closed!");
        }
    }
}
