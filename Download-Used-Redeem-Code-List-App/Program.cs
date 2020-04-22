using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;

namespace Download_Used_Redeem_Code_List_App
{
    class Program
    {
        public static Mutex mutex = null;
        public static System.Timers.Timer RepeatTiemr;
        public static string serverURL;

        static void Main(string[] args)
        {
            Console.WriteLine("Download-Used-Redeem-Code-List-App");

            const string appName = "Download-Used-Redeem-Code-List-App";
            bool createdNew;
            serverURL = LoadSetting("C:\\UID_Toolkit\\Global.json", "Server_URL");

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
                RepeatTiemr = new System.Timers.Timer(65000);

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
            string URL = serverURL + "public/api/download-used-redeem-code-list";
            WebClient wc = new WebClient();
            string html = wc.DownloadString(URL);
            File.WriteAllText("C:\\UID_Toolkit\\output\\used_redeem_codes.txt", html);

            Console.WriteLine("Used redeem code list file downloaded at: " + DateTime.Now.ToString());
        }

        // Load single property from .json file
        public static string LoadSetting(string filePath, string pName)
        {
            // Usage
            // string source_identifier_code = JSONExtension.LoadSetting("file path without extension", "property name");
            // string example1 = JSONExtension.LoadSetting("C:\\UID-TOOLKIT\\Settings", "ProjectFolder");

            JObject jsonObj = LoadJson(filePath);

            if (!jsonObj.ContainsKey(pName))
            {
                Console.WriteLine("Property not exist in setting : " + pName);
                return null;
            }
            else
            {
                return jsonObj[pName].ToString();
            }
        }

        // Load a .json file's text and parse to JSON format
        public static JObject LoadJson(string filePath)
        {
            // Read the file text
            string json = File.ReadAllText(filePath);

            // Deserialize into json format
            JObject jsonObj = JObject.Parse(json);

            return jsonObj;
        }
    }
}
