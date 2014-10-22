using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Lib;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Startup = Monitor.Web.Startup;

[assembly: OwinStartup(typeof(Startup), "Configuration")]

namespace Monitor.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();


        }
    }
}
