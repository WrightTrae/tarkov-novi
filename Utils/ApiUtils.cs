using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace tarkov_novi.Utils
{
    public static class ApiUtils
    {
        public static Data.Item getTarkovItem(string itemName)
        {
            var request = WebRequest.Create($@"https://tarkov-market.com/api/v1/item?q={itemName}&x-api-key=KrIX88iyD0YfiAYn");
            request.Method = "GET";

            using var webResponse = request.GetResponse();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();
            //JArray jsonArr = JArray.Parse(data);
            //JObject obj = (JObject)jsonArr.Children<JObject>()[0];
            //var item = new Item(obj);
            List<Data.Item> list = JsonConvert.DeserializeObject<List<Data.Item>>(data);
            if(list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }
    }
}
