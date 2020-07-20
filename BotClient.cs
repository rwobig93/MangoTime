using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime
{
    public class BotClient
    {
        private static readonly DiscordSocketClient _botInstance = new DiscordSocketClient();
        private BotClient()
        {

        }

        public static DiscordSocketClient Bot 
        { 
            get 
            { 
                return _botInstance; 
            } 
        }

        public static async Task StartBot()
        {
            try
            {
                _botInstance.Log += Program.Log;
                _botInstance.LoggedIn += Events.BotLoggedIn;
                _botInstance.LoggedOut += Events.BotLoggedOut;
                _botInstance.MessageReceived += Events.MessageReceived;
                await Program.Log("Running StartBot()");
                await _botInstance.LoginAsync(Discord.TokenType.Bot, Program.Config.BotToken);
                await _botInstance.StartAsync();
                await Program.Log("Finished Auth and Start");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
            }
        }

        public static async Task<bool> VerifyToken(string token)
        {
            try
            {
                await new DiscordSocketClient().LoginAsync(Discord.TokenType.Bot, token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token Failure: {ex.Message}");
                return false;
            }
        }
    }
}
