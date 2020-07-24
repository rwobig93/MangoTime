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
            await Context.Channel.SendMessageAsync(MT.CalculateMangoTime());
        }
    }
}
