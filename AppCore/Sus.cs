using Discord;
using Discord.Commands;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MangoTime.Bag;
using System.ComponentModel;
using Discord.WebSocket;

namespace MangoTime.AppCore
{
    public static class Sus
    {
        internal static List<string> DefaultSusList()
        {
            return new List<string>()
            {
                "https://filmdaily.co/wp-content/uploads/2020/10/among-1.jpg",
                "https://filmdaily.co/wp-content/uploads/2020/10/among-2-.jpg",
                "https://i.ytimg.com/vi/KEXny_kFd1o/maxresdefault.jpg"
            };
        }

        public static string GetSusImage(List<string> susImages)
        {
            return susImages[Util.RandomIndex(susImages.Count)];
        }

        internal static async Task StartSusDeliberationAsync(SocketCommandContext context, IUser mentionedUser)
        {
            var ogVC = context.Guild.VoiceChannels.First(x => x.Users.Contains(context.User));
            if (null == ogVC)
            {
                await context.Channel.SendMessageAsync("Was unable to find a valid user voice channel");
                await Events.Log($"Was unable to find a valid user voice channel for {context.User.Username}");
                return;
            }
            else
            {
                await Events.Log($"User VoiceChannel Found: {ogVC.Name} | {ogVC.Users.Count}");
            }

            var susVC = context.Guild.VoiceChannels.First(x => x.Name.ToLower().Contains("sus"));
            if (null == susVC)
            {
                await context.Channel.SendMessageAsync("Was unable to find a valid sus voice channel");
                await Events.Log("Was unable to find a valid sus voice channel");
                return;
            }
            else
            {
                await Events.Log($"Sus VoiceChannel Found: {susVC.Name} | {susVC.Users.Count}");
            }

            SusVote firstVote = new SusVote()
            {
                Voter = context.User,
                VotedSus = mentionedUser
            };

            SusDeliberation susDelib = new SusDeliberation()
            {
                TotalUsers = ogVC.Users.Count,
                Context = context,
                OGVoiceChannel = ogVC,
                SusVoiceChannel = susVC,
                FirstVote = firstVote
            };

            SusDeliberation(susDelib);
        }

        private static void SusDeliberation(SusDeliberation susDelib)
        {
            BackgroundWorker worker = new BackgroundWorker() { WorkerReportsProgress = true };
            worker.DoWork += async (ws, we) =>
            {
                while (DateTime.Now < susDelib.EndTime || susDelib.Votes.Count < susDelib.TotalUsers)
                {
                    if (DateTime.Now == susDelib.EndTime.Subtract(TimeSpan.FromSeconds(10)))
                    {
                        await susDelib.Context.Channel.SendMessageAsync("You have 10 seconds of deliberation time left to find the sus imposter!");
                    }
                }
                worker.ReportProgress(1);
            };
            worker.ProgressChanged += (ps, pe) =>
            {
                if (pe.ProgressPercentage == 1)
                {
                    // Put code to run on the UI thread when triggered
                }
            };
            worker.RunWorkerAsync();
        }
    }
}
