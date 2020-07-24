using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MangoTime
{
    public static class UI
    {
        internal static void AskForBotToken()
        {
            bool tokenRecvd = false;
            do
            {
                try
                {
                    Console.WriteLine("Please enter the bot token: ");
                    var botToken = Console.ReadLine();

                    Task<bool> verify = VerifyEntered(botToken);
                    tokenRecvd = verify.Result;
                    Task.WaitAll(verify);

                    if (tokenRecvd)
                        Program.Config.BotToken = botToken;
                }
                catch (Exception)
                {

                }
            }
            while (!tokenRecvd);

            Console.WriteLine("Token validated, moving on");
        }

        private static async Task<bool> VerifyEntered(string input)
        {
            return await BotClient.VerifyToken(input);
        }
    }
}
