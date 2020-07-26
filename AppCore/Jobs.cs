using System;
using System.Collections.Generic;
using System.Text;
using static MangoTime.Events;

namespace MangoTime.AppCore
{
    public static class Jobs
    {
        public static string GetCronString(CronTime time)
        {
            return time switch
            {
                CronTime.EveryMin => "*/1 * * * * command",
                CronTime.EveryMinSundays => "/1 * * * 0 command",
                _ => "*/1 * * * * command",
            };
        }

        public static void CheckOnMango()
        {
            Log("Running CheckOnMango()");

            // If it isn't sunday, skip job
            if (DateTime.Now.ToLocalTime().DayOfWeek != DayOfWeek.Sunday)
            {
                Log("We're not at the expected day yet, skipping job", Discord.LogSeverity.Debug);
                return;
            }
            // If it isn't 2pm yet, skip job
            if (DateTime.Now.ToLocalTime().Hour < 14)
            {
                Log("We're not at the expected time yet, skipping job", Discord.LogSeverity.Debug);
                return;
            }
            // If it's past 6pm then let's create a mango appearance as a default
            if (DateTime.Now.ToLocalTime().Hour > 18)
            {
                Log("Mango has gone over time, setting to default of 4 hour offset");
                MT.AddMangoAppearance();
                return;
            }

            // If we haven't captured a successfull mango appearance then let's check on mango
            if (Program.Config.MangoAppearances.FindAll(x => x.AppearanceTime.Date == DateTime.Now.Date).Count <= 0)
            {
                // If mango is online via a desktop client add a mango appearance
                if (MT.UserHasDesktopLogin("mango"))
                {
                    MT.AddMangoAppearance();
                }
            }
            else
            {
                Log("We've got a successful mango appearance, skipping mango check", Discord.LogSeverity.Debug);
            }
        }
    }
}
