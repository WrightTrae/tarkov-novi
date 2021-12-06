using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace tarkov_novi
{
    class TaskRunner
    {
        Timer myTimer;
        static volatile bool isRunning;
        MainWindow mainWindow;
        private Func<ParseTask> prepareTask;

        public TaskRunner(MainWindow main)
        {
            mainWindow = main;
            myTimer = new Timer();
            myTimer.Interval = 2500;
            myTimer.Elapsed += myTimer_Elapsed;
        }

        public TaskRunner(MainWindow main, Func<ParseTask> prepareTask) : this(main)
        {
            this.prepareTask = prepareTask;
        }

        public void Start()
        {
            myTimer.Start();
        }

        public void Stop()
        {
            myTimer.Stop();
            isRunning = false;
        }

        private async void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isRunning) return;
            isRunning = true;
            try
            {
                await ParseScreen();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { isRunning = false; }
        }
        public Task ParseScreen()
        {
            var parser = prepareTask();
            return Task.Run(() => parser.Parse());
        }
    }
}