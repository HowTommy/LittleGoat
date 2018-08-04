namespace LittleGoat.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Newtonsoft.Json;

    [HubName("seriePlayersHub")]
    public class SeriePlayersHub : Hub
    {
        public void BroadcastToAll(string message)
        {
            Clients.All.newMessageReceived(message);
        }
    }
}