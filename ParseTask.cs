using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tarkov_novi
{
    class ParseTask
    {
        public event EventHandler<Data.ThreadNotification<Data.ParseResults>> Notifier;

        public ParseTask()
        {

        }

        public void Parse()
        {
            Data.ParseResults results = Utils.ParserUtils.parseData();
            Notifier.Raise(this, results);
        }
    }
}
