using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangoTime.Bag
{
    public class SusDeliberation
    {
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now.Add(Program.Config.SusTimer);
        public int TotalUsers { get; set; }
        public SusVote FirstVote { get; set; }
        public List<SusVote> Votes { get; set; } = new List<SusVote>();
        public SocketCommandContext Context { get; set; }
        public SocketVoiceChannel OGVoiceChannel { get; set; }
        public SocketVoiceChannel SusVoiceChannel { get; set; }
    }

    public class SusVote
    {
        public IUser Voter { get; set; }
        public IUser VotedSus { get; set; }
        public DateTime TimeVoted { get; set; } = DateTime.Now;
    }
}
