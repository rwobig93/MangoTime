using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangoTime
{
    public static class MT
    {
        internal static UserStatus? GetUserStatusAsync(string usernameContains)
        {
            // Grab Mango user
            var user = GetUserContaining("main", usernameContains);

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
        }

        internal static SocketGuildUser GetUserContaining(string nameContains)
        {
            // Return SocketUser where channel name and username contains provided strings
            return BotClient.Bot.Guilds.FirstOrDefault().DefaultChannel.
                Users.FirstOrDefault(x => x.Username.ToLower().Contains(nameContains));
        }

        internal static SocketGuildUser GetUserContaining(string channelPrimaryContains, string nameContains)
        {
            // Return SocketUser where channel name and username contains provided strings
            return BotClient.Bot.Guilds.FirstOrDefault().Channels.
                FirstOrDefault(x => x.Name.ToLower().Contains(channelPrimaryContains)).
                Users.FirstOrDefault(x => x.Username.ToLower().Contains(nameContains));
        }
    }
}
