using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime
{
    public static class Events
    {
        internal static Task BotLoggedIn()
        {
            Program.Log("Bot logged in", Discord.LogSeverity.Info);
            return Task.CompletedTask;
        }

        internal static Task BotLoggedOut()
        {
            Program.Log("Bot logged out", Discord.LogSeverity.Info);
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
            Config.SaveConfig();
            Console.WriteLine("Closing!");
        }
    }
}
