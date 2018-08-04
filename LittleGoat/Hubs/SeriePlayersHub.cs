namespace LittleGoat.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("seriePlayersHub")]
    public class SeriePlayersHub : Hub
    {
        public void JoinGroup(string group)
        {
            Groups.Add(Context.ConnectionId, group);
        }

        public void QuitGroup(string group)
        {
            Groups.Remove(Context.ConnectionId, group);
        }

        public void BroadcastToGroup(string group, string message)
        {
            Clients.Group(group).newMessageReceived(message);
        }
    }
}