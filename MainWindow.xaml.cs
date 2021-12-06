using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageUI = System.Windows.Controls.Image;
using Tesseract;
using System.Diagnostics;
using System.Threading;
using Microsoft.Web.WebView2.Wpf;

namespace tarkov_novi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskRunner taskRunner;
        Boolean urlSet = false;

        public MainWindow()
        {
            InitializeComponent();
            taskRunner = new TaskRunner(this, PrepareTask);
            taskRunner.Start();
        }

        ParseTask PrepareTask()
        {
            var parser = new ParseTask();
            parser.Notifier += (s, e) => Dispatcher.Invoke(delegate ()
            {
                Debug.WriteLine("UPDATE UI");

                var windowStatus = e.Args.windowStatus;
                if (windowStatus == "MINIMIZED")
                {
                    landingLbl.Content = "Game window is minimized or not found, go fuck yourself for help";
                }
                else
                {
                    landingLbl.Content = "Enter the Prepare to Escape confirmation screen to pull up the map or place an item window in the upper left corner to pull up item details";
                }

                var mapImage = e.Args.mapImageUri;
                ImageUI mapImg = this.FindName("mapImage") as ImageUI;
                if(mapImage != null)
                {
                    try
                    {
                        mapImg.Visibility = Visibility.Visible;
                        mapImg.Source = new BitmapImage(new Uri(mapImage));
                    }
                    catch (Exception er)
                    {
                        mapImg.Visibility = Visibility.Hidden;
                        Debug.WriteLine(er);
                    }
                }


                //var tarkItem = e.Args.item;
                //WebBrowser itemBrowser = this.FindName("itemBrowser") as WebBrowser;
                //if (tarkItem != null && tarkItem.meanConf > .65)
                //{
                //    itemBrowser.Visibility = Visibility.Visible;
                //    var itemUrl = $@"https://tarkov-market.com/item/{tarkItem.name.Replace(' ', '_')}";
                //    Debug.WriteLine(itemUrl);
                //    itemBrowser.Navigate(itemUrl);
                //}
                //else
                //{
                //    itemBrowser.Visibility = Visibility.Collapsed;
                //    itemBrowser.Navigate((Uri)null);
                //}

                var tarkItem = e.Args.item;
                Grid itemGrid = this.FindName("itemGrid") as Grid;
                ImageUI itemIcon = this.FindName("itemIcon") as ImageUI;
                Label itemName = this.FindName("itemName") as Label;
                Label itemPricePerSlot = this.FindName("itemPricePerSlot") as Label;
                Label itemSlotSize = this.FindName("itemSlotSize") as Label;
                Label itemPrice = this.FindName("itemPrice") as Label;
                Label itemAvgPrice = this.FindName("itemAvgPrice") as Label;
                WebView2 itemBrowser = this.FindName("itemBrowser") as WebView2;
                itemBrowser.EnsureCoreWebView2Async();
                if (tarkItem == null)
                {
                    itemGrid.Visibility = Visibility.Collapsed;
                    itemBrowser.Visibility = Visibility.Collapsed;
                    urlSet = false;
                    itemName.Content = "";
                    itemPricePerSlot.Content = "";
                    itemSlotSize.Content = "";
                    itemPrice.Content = "";
                    itemAvgPrice.Content = "";
                }
                else
                {
                    itemBrowser.Visibility = Visibility.Visible;
                    itemGrid.Visibility = Visibility.Visible;
                    if(!urlSet && itemBrowser != null && itemBrowser.CoreWebView2 != null)
                    {
                        urlSet = true;
                        itemBrowser.CoreWebView2.Navigate(tarkItem.wikiLink);
                    }
                    itemIcon.Source = new BitmapImage(new Uri(tarkItem.icon));
                    itemName.Content = tarkItem.name;
                    itemPricePerSlot.Content = tarkItem.getPricePerSlot();
                    itemSlotSize.Content = tarkItem.slots;
                    itemPrice.Content = tarkItem.price;
                    itemAvgPrice.Content = tarkItem.avg7daysPrice;
                }
            });
            return parser;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Utils.ParserUtils.parseData();
        }
    }
}
