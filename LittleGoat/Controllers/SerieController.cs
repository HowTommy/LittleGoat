namespace LittleGoat.Controllers
{
    using LittleGoat.DataAccess;
    using LittleGoat.Filter;
    using LittleGoat.Models;
    using LittleGoat.ViewModels;
    using System;
    using System.Collections.Generic;
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

                if(TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];
                }

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
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        [IsAuthenticated]
        public ActionResult New(NewSerieViewModel model)
        {
            var playerId = GetPlayerId();
            Random r = new Random();
            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var serie = entities.Serie.Single(p => p.Id == model.Key);
                if (serie.CreatorId != playerId)
                {
                    TempData["ErrorMessage"] = Resources.you_cant_start_the_game_only_creator;
                    return RedirectToAction("New", "Serie", new { key = model.Key });
                }

                var players = serie.SeriePlayers.ToList();
                if (players.Count < 2)
                {
                    TempData["ErrorMessage"] = Resources.you_cant_play_alone;
                    return RedirectToAction("New", "Serie", new { key = model.Key });
                }

                serie.Started = true;
                entities.SaveChanges();

                foreach (var game in entities.Game.Where(p => p.SerieId == serie.Id))
                {
                    game.Ended = true;
                }
                entities.SaveChanges();

                string cardGiverId = players[r.Next(0, players.Count)].PlayerId;

                var newGame = new Game()
                {
                    CreationDate = DateTime.UtcNow,
                    Ended = false,
                    LittleGoatCallerId = null,
                    SerieId = serie.Id,
                    CardGiverId = cardGiverId,
                    NextToPlayId = cardGiverId
                };
                entities.Game.Add(newGame);
                entities.SaveChanges();

                var cards = Cards.AllCards.GetAllCards();
                List<GameCard> gameCards = new List<GameCard>();
                int position = 0;

                while (cards.Any())
                {
                    var nextCard = cards[r.Next(0, cards.Count)];
                    cards.Remove(nextCard);
                    gameCards.Add(new GameCard()
                    {
                        Value = (int)nextCard.Value,
                        Symbol = (int)nextCard.Symbol,
                        Position = position,
                        IsCover = false,
                        PlayerId = null,
                        GameId = newGame.Id,
                        FileName = nextCard.FileName,
                    });
                    position++;
                }
                entities.GameCard.AddRange(gameCards);
                entities.SaveChanges();

                var cardsToDistribute = gameCards.OrderBy(p => p.Position).Take(players.Count * 4).ToArray();
                for (int i = 0; i < cardsToDistribute.Length; i++)
                {
                    var targetPlayer = players.Single(p => p.Position == i % players.Count);
                    cardsToDistribute[i].PlayerId = targetPlayer.PlayerId;
                }
                entities.SaveChanges();
            }

            return RedirectToAction("Play", "Game", new { key = model.Key });
        }
    }
}