using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BettinCoTestTask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GisMeteo10Days prevData = null;
            GisMeteo10Days newData = null;

            while (true)
            {
                var response = await GetHtmlCodeAsync("https://www.gismeteo.ru/weather-miami-7063/10-days/");
                newData = new GisMeteo10Days(response);
                if (newData != prevData)
                {
                    //add to db;
                    Console.WriteLine("!=");
                }
                else
                {
                    Console.WriteLine("==");
                }
                await Task.Delay(5000);
                prevData = newData;
            }
        }

        private static async Task<string> GetHtmlCodeAsync(string uri)
        {
            string htmlCode = String.Empty;
            using (WebClient client = new WebClient())
            {
                try
                {
                    htmlCode = await client.DownloadStringTaskAsync(uri);
                    await File.WriteAllTextAsync(@"../file.html", htmlCode);
                }
                catch
                {
                    throw new Exception("Failed to download html page");
                }
            }
            return htmlCode;
        }
    }
}
