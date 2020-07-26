using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MangoTime.Events;

namespace MangoTime
{
    public class BotClient
    {
        private readonly DiscordSocketClient _botInstance = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = Discord.LogSeverity.Info
        });
        private readonly CommandService _commands = new CommandService(new CommandServiceConfig
                {
                    LogLevel = Discord.LogSeverity.Info,
                    CaseSensitiveCommands = false
                });
        private readonly IServiceProvider _services;

        public BotClient()
        {
            // Initialize dependency injection
            // _services = ConfigureServices();
        }

        public DiscordSocketClient Client { get { return _botInstance; } }

        public async Task StartBot()
        {
            try
            {
                await Log("Running StartBot()", Discord.LogSeverity.Info);

                // Assign bot client event handlers
                _botInstance.Log += Log;
                _botInstance.LoggedIn += Events.BotLoggedIn;
                _botInstance.LoggedOut += Events.BotLoggedOut;
                //_botInstance.MessageReceived += Events.MessageReceived;

                // Initialize command service
                _commands.Log += Log;
                await InitializeCommands();

                // Login and start bot client
                await _botInstance.LoginAsync(Discord.TokenType.Bot, Program.Config.BotToken);
                await _botInstance.StartAsync();
                await Log("Finished Auth and Start", Discord.LogSeverity.Info);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
            }
        }

        private async Task InitializeCommands()
        {
            var handler = new CommandHandler(Client, _commands);
            await handler.InstallCommandsAsync();
        }

        private static IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection();
            // .AddSingleton(new InjectedServiceClass());
            return map.BuildServiceProvider();
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
