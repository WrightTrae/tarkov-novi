using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tarkov_novi
{
    static class ThreadNotifier
    {
        public static void Raise<T>(this EventHandler<Data.ThreadNotification<T>> theEvent, object sender, T args)
        {
            if (theEvent != null)
                theEvent(sender, new Data.ThreadNotification<T>(args));
        }
    }
}
