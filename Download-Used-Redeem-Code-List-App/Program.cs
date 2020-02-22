using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;

namespace Download_Used_Redeem_Code_List_App
{
    class Program
    {
        private static Mutex mutex = null;
        private static System.Timers.Timer RepeatTiemr;

        static void Main(string[] args)
        {
            const string appName = "Download-Used-Redeem-Code-List-App";
            bool createdNew;

            // Create app unique identifier
            mutex = new Mutex(true, appName, out createdNew);

            // Check if other objects with the same identifier was already created.
            // If so, other instances of this app is already running, current instance need to be closed.
            if (!createdNew)
            {
                Console.WriteLine(appName + " is already running! Exiting the application.");
            }
            else
            {
                // Create a timer to repeat run the downlaod txt file process
                RepeatTiemr = new System.Timers.Timer(60000);

                // Timer event
                RepeatTiemr.Elapsed += DownloadFile;

                // Enable the timer
                RepeatTiemr.Enabled = true;

                // Prevent app from closes itself
                Console.ReadKey();
            }

            return;
        }

        private static void DownloadFile(object source, ElapsedEventArgs e)
        {
            string URL = "https://191209-my-gentingvm.unicom-interactive-digital.com/public/api/download-used-redeem-code-list";
            WebClient wc = new WebClient();
            string html = wc.DownloadString(URL);
            File.WriteAllText("C:\\Vending-Machine-Controller\\used_redeem_codes.txt", html);

            Console.WriteLine("Downloaded file at: " + DateTime.Now.ToShortTimeString());
        }
    }
}
