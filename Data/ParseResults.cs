using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace tarkov_novi.Data
{
    class ParseResults
    {
        public ParseResults(Item item, string mapImageUri, string windowStatus)
        {
            this.item = item;
            this.mapImageUri = mapImageUri;
            this.windowStatus = windowStatus;
        }
        public Item item
        {
            get;
            set;
        }
        public string mapImageUri
        {
            get;
            set;
        }
        public string windowStatus
        {
            get;
            set;
        }
    }
}
