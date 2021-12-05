using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tarkov_novi.Data
{
    class ThreadNotification<T>
    {
        public ThreadNotification(T args)
        {
            Args = args;
        }

        public T Args { get; }
    }
}
