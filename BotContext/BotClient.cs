using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime
{
    public class BotClient
    {
        private static readonly DiscordSocketClient _botInstance = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = Discord.LogSeverity.Info
        });
        private static readonly CommandService _commands = new CommandService(new CommandServiceConfig
                {
                    LogLevel = Discord.LogSeverity.Info,
                    CaseSensitiveCommands = false
                });
        private readonly IServiceProvider _services;

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
                //_botInstance.MessageReceived += Events.MessageReceived;

                await Program.Log("Running StartBot()", Discord.LogSeverity.Info);
                await _botInstance.LoginAsync(Discord.TokenType.Bot, Program.Config.BotToken);
                await _botInstance.StartAsync();
                await Program.Log("Finished Auth and Start", Discord.LogSeverity.Info);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
            }
        }

        private async Task InitializeCommands()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
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
