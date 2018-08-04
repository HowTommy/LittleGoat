namespace LittleGoat.Controllers
{
    using LittleGoat.DataAccess;
    using LittleGoat.Filter;
    using LittleGoat.Models;
    using LittleGoat.ViewModels;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class SerieController : BaseController
    {
        public ActionResult Create()
        {
            string playerId = Request.Cookies["playerId"]?.Value;

            if (string.IsNullOrWhiteSpace(playerId))
            {
                return RedirectToAction("ChooseName", "Account", new { returnUrl = Url.Action("Create", "Serie", null, "http") });
            }

            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var creator = entities.Player.FirstOrDefault(p => p.Id == playerId);

                if (creator == null)
                {
                    CleanCookie();
                    return RedirectToAction("ChooseName", new { returnUrl = Url.Action("Create", "Serie", null, "http") });
                }

                var serie = new Serie() { CreationDate = DateTime.UtcNow, Ended = false, CreatorId = creator.Id, Id = Guid.NewGuid().ToString(), Started = false };
                entities.Serie.Add(serie);
                serie.SeriePlayers.Add(new SeriePlayers() { SerieId = serie.Id, PlayerId = playerId, Position = 0 });

                entities.SaveChanges();

                return RedirectToAction("New", "Serie", new { key = serie.Id });
            }
        }

        [IsAuthenticated]
        public ActionResult New(string key)
        {
            var playerId = GetPlayerId();
            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var serie = entities.Serie.Single(p => p.Id == key);
                var player = entities.Player.Single(p => p.Id == playerId);
                var seriePlayers = serie.SeriePlayers.ToList();
                int position = seriePlayers.Max(p => p.Position) + 1;

                if (!seriePlayers.Any(p => p.PlayerId == playerId))
                {
                    serie.SeriePlayers.Add(new SeriePlayers() { PlayerId = player.Id, SerieId = key, Position = position });
                }

                entities.SaveChanges();

                var players = entities.SeriePlayers.Where(p => p.SerieId == key).Select(p => p.Player.Name).ToList();

                var lastChatMessages = entities.SerieChat
                    .Where(p => p.SerieId == key)
                    .OrderByDescending(p => p.Id)
                    .Select(p => new ChatMessage() { Id = p.Id, Date = p.Date, Message = p.Message, PlayerId = p.PlayerId, PlayerName = p.Player.Name })
                    .Take(30)
                    .OrderBy(p => p.Date)
                    .ToList();

                return View(new NewSerieViewModel()
                {
                    Key = key,
                    IsCreator = serie.CreatorId == playerId,
                    Players = players,
                    CurrentPlayerName = player.Name,
                    LastChatMessages = lastChatMessages
                });
            }
        }
    }
}