using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime.BotContext
{
    public class MangoModule : ModuleBase<SocketCommandContext>
    {
        [Command("mangotime")]
        [Summary("Displays current mangotime")]
        [Alias("mt")]
        public async Task MangoTime()
        {
            var embed = new EmbedBuilder
            {
                Title = "Mango Time!",
                Color = Color.Teal,
                Url = "https://github.com/rwobig93/MangoTime",
                ImageUrl = "https://media.giphy.com/media/IbaWetIO1PA9LKN5mj/giphy.gif",
                Description = MT.CalculateMangoTime()
            };
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        //[Command("test")]
        //public async Task Test()
        //{
        //    MT.AddMangoAppearanceTest();
        //    var test1 = Program.Config.MangoAppearances;
        //    var embed = new EmbedBuilder
        //    {
        //         Title = "Mango Time!",
        //          Color = Color.Teal,
        //           Url = "https://media.giphy.com/media/IbaWetIO1PA9LKN5mj/giphy.gif",
        //            ImageUrl = "https://media.giphy.com/media/IbaWetIO1PA9LKN5mj/giphy.gif",
        //             Description = MT.CalculateMangoTime()
        //    };
        //    await Context.Channel.SendMessageAsync(embed: embed.Build());
        //}
    }
}
