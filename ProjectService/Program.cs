using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectService
{
    internal class Program
    {
        static async void SendApps()
        {
            // TODO: replace localhost with whatever the address of the master server is
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            List<string> installedApps = new List<string>();
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                    {
                        var appName = subkey.GetValue("DisplayName");
                        if (appName == null) continue;
                        Console.WriteLine(appName);
                        installedApps.Add(appName.ToString());
                    }
                }
            }
            HttpClient client = new HttpClient();
            var postData = new Dictionary<string, string>
            { {"apps", JsonConvert.SerializeObject(installedApps)} };
            var x = await client.PostAsync("https://localhost:44340/API/SubmitApps", new FormUrlEncodedContent(postData));
            Console.WriteLine(x.ToString());
        }

        static void Main(string[] args)
        {
            while (true)
            {
                SendApps();
                System.Threading.Thread.Sleep(14400000);
            }
        }
    }
}
