using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime
{
    public static class Commands
    {
        internal static async Task HandleMessage(SocketMessage arg)
        {
            if (arg.Content.Contains(" "))
            {
                // Handle commands w/ 1 or more args
                await CommandWithArgs(arg);
            }
            else
            {
                // Handle commands w/o any args
                await CommandWithoutArgs(arg);
            }
        }

        private static async Task CommandWithoutArgs(SocketMessage arg)
        {
            switch (arg.Content.ToLower())
            {
                case "!mt":
                    await MangoTimeAsync(arg);
                    break;
                case "!randy":
                    await RandySavageAsync(arg);
                    break;
                case "!mango":
                    await MangoStatus(arg);
                    break;
                default:
                    break;
            }
        }

        private static async Task CommandWithArgs(SocketMessage arg)
        {
            if (arg.Content.ToLower().StartsWith("!status"))
            {
                await UserListStatusAsync(arg);
            }
        }

        private static async Task UserListStatusAsync(SocketMessage arg)
        {
            var split = arg.Content.ToLower().Split(" ");
            await arg.Channel.SendMessageAsync(UserStatus(split, arg));
        }

        private static string UserStatus(string[] users, SocketMessage arg)
        {
            int index = 0;
            string userStatusList = "";
            foreach (var user in users)
            {
                if (index != 0)
                {
                    var userFound = MT.GetUserContaining(user);
                    var userName = null == userFound ? user : userFound.Username;
                    var userStatus = null == userFound ? "not found" : userFound.Status.ToString();
                    userStatusList += $"{userName} :: {userStatus}{Environment.NewLine}";
                }
                index++;
            }
            return userStatusList;
        }

        private static async Task MangoTimeAsync(SocketMessage arg)
        {
            await arg.Channel.SendMessageAsync("MAAAANGOOO TIIIIIIIIIIIIME!!!!");
        }

        private static async Task RandySavageAsync(SocketMessage arg)
        {
            await arg.Channel.SendMessageAsync("I've been to the top of the mountain, AND I'M GOUHN BAAAACK!!!");
        }

        private static async Task MangoStatus(SocketMessage arg)
        {
            await arg.Channel.SendMessageAsync(null == MT.GetUserStatusAsync("mango") ? "User not found containing \"mango\"" : MT.GetUserStatusAsync("mango").ToString());
        }
    }
}
