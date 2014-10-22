using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Lib;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Monitor.Server.Startup), "Configuration")]

namespace Monitor.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.UseErrorPage();

            builder.Map("/signalr", map =>
            {
                var hubConfiguration = new HubConfiguration
                {

                    EnableJavaScriptProxies = true,
                    EnableDetailedErrors = true
                };
                map.UseCors(CorsOptions.AllowAll)
                    .RunSignalR(hubConfiguration);

            });
            GlobalHost.TraceManager.Switch.Level = SourceLevels.Information;
            Task.Run(() => CheckAlarmsWorker.Work());
        }
    }


    public class CheckAlarmsWorker
    {
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(4);
        private static readonly Random Random = new Random();
        private static readonly Guid Topolino = Guid.NewGuid();
        private static readonly Guid Minnie = Guid.NewGuid();
        private static readonly Guid Bottleneck = Guid.NewGuid();
        public static void Work()
        {
            while (true)
            {
                DoCheck();
                Thread.Sleep(Interval);
            }
        }

        private static void DoCheck()
        {
            try
            {
                
                var steps = new[] {"data import", "dept generation", "catalog bilder", "stockgrabber", "dept export", "solr main", "solr fayt"};
                var alarmStatusRows = new[]
                {
                    new {Id=Topolino, Division="Topolino", Version="1", Environment = "Prod",  Step = steps[Random.Next(0,6)], Status = 0 },
                    new {Id=Minnie, Division="Minnie", Version="10",Environment = "Preview",  Step = steps[Random.Next(0,6)], Status = 1 },
                    new {Id=Bottleneck, Division="Bottleneck", Version="4",Environment = "Dev",  Step = steps[Random.Next(0,6)], Status = 2 },

                };
                var contexts = alarmStatusRows.ToDictionary(k => String.Format("{0:N}@{1:N}@{2:N}@{3:N}@{4:N}", k.Id, k.Division, k.Version, k.Environment, k.Step), v => v.Status);
                var data = new Dictionary<string, int>(contexts, StringComparer.Ordinal);
                BroadcastMessageToClients(data);
            }
            catch (Exception ex)
            {
            }
        }

        private static void BroadcastMessageToClients(IDictionary<string, int> data)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CatalogHub>();

            context.Clients.All.onReceiveAlarms(data);
        }
    }
}
