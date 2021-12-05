using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Tesseract;

namespace tarkov_novi.Utils
{
    public static class ImageUtils
    {
        public static BitmapImage ToWpfImage(this System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();  // no using here! BitmapImage will dispose the stream after loading
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage ix = new BitmapImage();
            ix.BeginInit();
            ix.CacheOption = BitmapCacheOption.OnLoad;
            ix.StreamSource = ms;
            ix.EndInit();
            return ix;
        }
        public static Bitmap cropAtRect(this Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -r.X, -r.Y);
                return nb;
            }
        }
        public static Image drawOnImage(Image image, Rectangle rect)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                Pen shadowBrush = new Pen(Color.Red, 5);
                g.DrawRectangle(shadowBrush, rect);
            }
            return image;
        }
        public static (List<string>, float) parseMapName(Image image)
        {
            string[] MAP_NAMES = {
                "reserve",
                "customs",
                "factory",
                "interchange",
                "shoreline",
                "woods",
                "the lab"
            };
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)(image.Width / 2.06), (int)(image.Height / 11), (int)(image.Width / 7), (int)(image.Height / 23));
            return parseAtRect(image, rect, MAP_NAMES);
        }
        public static (List<string>, float) parseItemName(Image image)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(26, -5, (int)(image.Width / 5), (int)(image.Height / 50));
            return parseAtRect(image, rect);
        }
        public static (List<string>, float) parseAtRect(Image image, Rectangle rect, string[] possibleValues = null)
        {
            Bitmap bitmapImg = new Bitmap(image);
            Bitmap cropedBitmap = cropAtRect(bitmapImg, rect);

            List<string> allLines = new List<string>();
            float meanConf = 0;
            try
            {
                using (var engine = new TesseractEngine(@"C:\Users\tdog1\source\repos\tarkov-novi\tessdata", "eng", EngineMode.Default))
                {
                    using (var page = engine.Process(cropedBitmap, PageSegMode.SingleLine))
                    {
                        var text = page.GetText();
                        meanConf = page.GetMeanConfidence();
                        Debug.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();

                            do
                            {
                                do
                                {
                                    do
                                    {
                                        string parsedText = iter.GetText(PageIteratorLevel.TextLine).ToLower();
                                        if (possibleValues != null)
                                        {
                                            foreach (string val in possibleValues)
                                            {
                                                if (parsedText.Contains(val))
                                                {
                                                    return (new List<string> { val }, meanConf);
                                                }
                                            }
                                            return (new List<string> { "" }, meanConf);
                                        }
                                        allLines.Add(parsedText);
                                        //Debug.Write(iter.GetText(PageIteratorLevel.Word));
                                        //Debug.Write(" ");

                                        //if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                        //{
                                        //    Debug.WriteLine(":: final ::");
                                        //}
                                    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                            } while (iter.Next(PageIteratorLevel.Block));
                        }
                    }
                }
            }
            catch (Exception er)
            {
                Trace.TraceError(er.ToString());
                Debug.WriteLine("Unexpected Error: " + er.Message);
                Debug.WriteLine("Details: ");
                Debug.WriteLine(er.ToString());
            }
            return (allLines, meanConf);
        }
    }
}
