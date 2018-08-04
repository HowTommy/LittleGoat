namespace LittleGoat.Hubs
{
    using LittleGoat.DataAccess;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using System;
    using System.Linq;
    using System.Web;

    [HubName("serieHub")]
    public class SerieHub : Hub
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

        public void BroadcastChatMessageToGroup(string group, string message)
        {
            var playerId = HttpContext.Current.Request.Cookies["playerId"].Value;
            message = HttpUtility.HtmlEncode(message);

            using (var entities = new LittleGoatEntities())
            {
                var player = entities.Player.Single(p => p.Id == playerId);

                var lastMessage = entities.SerieChat.Where(p => p.SerieId == group).OrderByDescending(p => p.Id).FirstOrDefault();

                if (lastMessage != null && lastMessage.PlayerId == player.Id)
                {
                    lastMessage.Message += "<br />" + message;

                    entities.SaveChanges();

                    Clients.Group(group).newChatMessageUpdated(lastMessage.Id, lastMessage.Message);
                }
                else
                {
                    var serieChat = new SerieChat()
                    {
                        Date = DateTime.UtcNow,
                        Message = message,
                        PlayerId = player.Id,
                        SerieId = group
                    };

                    entities.SerieChat.Add(serieChat);
                    entities.SaveChanges();

                    Clients.Group(group).newChatMessageReceived(serieChat.Id, serieChat.Date.ToString("dd/MM HH:mm"), player.Name, HtmlHelperExtensions.GetPlaceHolderLinkForAvatar(player.Name), message);
                }
            }
        }

        public static void StartGame(string key)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SerieHub>();
            context.Clients.Group(key).startGame();
        }
    }
}