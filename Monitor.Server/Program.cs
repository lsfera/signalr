using System;
using Microsoft.Owin.Hosting;

namespace Monitor.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var url = "http://localhost:8181";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
}


