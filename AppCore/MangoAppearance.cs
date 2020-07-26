using System;
using System.Collections.Generic;
using System.Text;

namespace MangoTime.AppCore
{
    public class MangoAppearance
    {
        public int RecordNumber { get; set; }
        public DateTime AppearanceTime { get; set; }
        public DateTime ScheduledTime { get; set; }
        public TimeSpan TimeOffset { get; set; }
        public bool PlayedDarkSouls { get; set; } = false;
    }
}
