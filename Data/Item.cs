using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tarkov_novi.Data
{
    public class Item
    {
        public Item()
        {

        }
        //public Item(JObject item)
        //{
        //    this.name = (string)item[nameof(name)];
        //    this.price = (string)item[nameof(price)];
        //    this.slots = (string)item[nameof(slots)];
        //    this.avg7daysPrice = (string)item[nameof(avg7daysPrice)];
        //    this.icon = (string)item[nameof(icon)];
        //}
        public string name
        {
            get;
            set;
        }
        public string price
        {
            get;
            set;
        }
        public string slots
        {
            get;
            set;
        }
        public string avg7daysPrice
        {
            get;
            set;
        }
        public string icon
        {
            get;
            set;
        }
        public string link
        {
            get;
            set;
        }
        public float meanConf
        {
            get;
            set;
        }
        public double getPricePerSlot()
        {
            return double.Parse(price) / double.Parse(slots);
        }
    }
}
