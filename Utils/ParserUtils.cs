﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace tarkov_novi.Utils
{
    static class ParserUtils
    {
        public static Data.ParseResults parseData()
        {
            Data.ParseResults parseResults = new Data.ParseResults(null, null, null);

            IntPtr windowHandle = WindowUtils.getGameHandle();
            if (windowHandle.ToInt32() == 0x0000000000000000)
            {
                parseResults.windowStatus = "MINIMIZED";
            }
            else
            {
                parseResults.windowStatus = "IN_FOCUS";

                Image screenshot = WindowUtils.CaptureWindow(windowHandle);
                //var screenshot = Image.FromFile(@"C:\Users\tdog1\source\repos\tarkov-novi\test-assets\test-item.PNG");

                var mapName = ParseMapName(screenshot);
                Debug.WriteLine(mapName);
                if (mapName != "")
                {
                    parseResults.mapImageUri = $@"C:\Users\tdog1\source\repos\tarkov-novi\maps\{mapName}.png";

                }
                else
                {
                    parseResults.mapImageUri = null;
                }

                var itemDetails = ParseItemDetails(screenshot);
                parseResults.item = itemDetails;
            }
            return parseResults;
        }

        private static String ParseMapName(Image screenshot)
        {
            var mapParseResults = ImageUtils.parseMapName(screenshot);
            List<string> parsedMapName = mapParseResults.Item1;
            float meanConf = mapParseResults.Item2;
            Debug.WriteLine($"Map Parse:: Confidence={meanConf} ParsedValues={String.Join(", ", parsedMapName)}");
            if(parsedMapName.Count > 0)
            {
                return parsedMapName[0];
            } else
            {
                return "";
            }
        }

        private static Data.Item ParseItemDetails(Image screenshot)
        {
            var itemParseResults = ImageUtils.parseItemName(screenshot);
            List<string> parsedItemName = itemParseResults.Item1;
            float meanConf = itemParseResults.Item2;
            Debug.WriteLine($"Item Parse:: Confidence={meanConf} ParsedValues={String.Join(", ", parsedItemName)}");
            if (meanConf > .70)
            {
                var item = ApiUtils.getTarkovItem(parsedItemName[0][1..]);
                //item.meanConf = meanConf;
                return item;
            }
            else
            {
                return null;
            }

        }
    }
}
