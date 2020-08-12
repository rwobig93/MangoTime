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

            var userSearch = "mango"; // mango
            var now = DateTime.Now.ToLocalTime();
            var todayAppearances = Program.Config.MangoAppearances.FindAll(x => x.AppearanceTime.Date == DateTime.Now.Date);

            // If it isn't sunday, skip job
            if (now.DayOfWeek != DayOfWeek.Sunday) { return; }

            // If we have a mango appearance for today return
            if (todayAppearances.Count >= 1) { return; }

            // If it isn't 2pm yet, skip job
            if (now.Hour < 14)
            {
                Log("We're not at the expected time yet, skipping job", Discord.LogSeverity.Debug);
                return;
            }

            // If it's past 6pm then let's create a mango appearance as a default
            if (now.Hour > 18)
            {
                Log("Mango has gone over time, setting to default of 4 hour offset");
                MT.AddMangoAppearance();
                return;
            }

            // If mango is online via a desktop client add a mango appearance
            if (MT.UserHasDesktopLogin(userSearch))
            {
                MT.AddMangoAppearance();
            }
            else if (MT.UserHasMobileLogin(userSearch))
            {
                Log("We've got a successful mango appearance, but it was on mobile", Discord.LogSeverity.Debug);
            }
            else if (MT.UserHasWebLogin(userSearch))
            {
                Log("We've got a successful mango appearance, but it was on web", Discord.LogSeverity.Debug);
            }
            else
            {
                Log("We don't have an active user, but we checked", Discord.LogSeverity.Debug);
            }
        }
    }
}
