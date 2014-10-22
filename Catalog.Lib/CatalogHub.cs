using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Catalog.Lib
{
    [HubName("catalogHub")]
    public class CatalogHub : Hub
    {
    }
}