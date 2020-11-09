using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MangoTime.AppCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MangoTime.BotContext
{
    public class SusModule : ModuleBase<SocketCommandContext>
    {
        [Command("suspicious")]
        [Summary("Someone has been actin sus")]
        [Alias("sus")]
        public async Task MangoTime(IUser user)
        {
            var embed = new EmbedBuilder
            {
                Title = "There is 1 Imposter among us",
                Color = Color.LightOrange,
                Url = "https://github.com/rwobig93/MangoTime",
                ImageUrl = Sus.GetSusImage(Program.Config.SusImages),
                Description = $"{user.Mention} You've been accused of acting sus, the meeting has begun!"
            };
            await Context.Channel.SendMessageAsync(embed: embed.Build());

            await Sus.StartSusDeliberationAsync(Context, user);
        }
    }
}
