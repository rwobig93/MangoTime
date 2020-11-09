using System;
using System.Collections.Generic;
using System.Text;

namespace MangoTime.Bag
{
    public static class Util
    {
        public static int RandomIndex(int max)
        {
            var random = new Random();
            return random.Next(0, max - 1);
        }
    }
}
