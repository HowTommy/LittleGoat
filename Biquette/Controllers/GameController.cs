namespace Biquette.Controllers
{
    using Biquette.DataAccess;
    using Biquette.Filter;
    using Biquette.Models;
    using Biquette.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class GameController : DefaultController
    {
        private const int NB_SECONDS_TO_SHOW_FIRST_TWO_CARDS = 6;

        // GET: Game
        public ActionResult Create()
        {
            string playerId = Request.Cookies["playerId"]?.Value;

            if (string.IsNullOrWhiteSpace(playerId))
            {
                return RedirectToAction("ChooseName", new { returnUrl = Url.Action("Create", "Game", null, "http") });
            }

            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var creator = entities.Player.FirstOrDefault(p => p.Id == playerId);

                if (creator == null)
                {
                    CleanCookie();
                    return RedirectToAction("ChooseName", new { returnUrl = Url.Action("Create", "Game", null, "http") });
                }

                var serie = new Serie() { CreationDate = DateTime.UtcNow, Ended = false, CreatorId = creator.Id, Id = Guid.NewGuid().ToString(), Started = false };
                entities.Serie.Add(serie);
                serie.SeriePlayers.Add(new SeriePlayers() { SerieId = serie.Id, PlayerId = playerId, Position = 0 });

                entities.SaveChanges();

                return RedirectToAction("NewGame", new { key = serie.Id });
            }
        }

        public ActionResult ChooseName(string returnUrl)
        {
            return View(new ChooseNameViewModel() { ReturnUrl = returnUrl });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChooseName(ChooseNameViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ViewBag.ErrorMessage = Resources.name_cant_be_empty;
                return View(model);
            }

            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                Player player;

                string playerId = Request.Cookies["playerId"]?.Value;
                if (playerId == null || entities.Player.FirstOrDefault(p => p.Id == playerId) == null)
                {
                    player = new Player() { Id = Guid.NewGuid().ToString(), Name = model.Name };
                    entities.Player.Add(player);
                    Response.Cookies["playerId"].Value = player.Id;
                    Response.Cookies["playerId"].Expires = DateTime.UtcNow.AddYears(10);
                }
                else
                {
                    entities.Player.Single(p => p.Id == playerId).Name = model.Name;

                }
                entities.SaveChanges();


            }

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [IsAuthenticated]
        public ActionResult NewGame(string key)
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

                return View(new NewGameViewModel() { Key = key, IsCreator = serie.CreatorId == playerId });
            }
        }

        [HttpPost]
        public ActionResult GetCurrentPlayers(string key)
        {
            try
            {
                using (LittleGoatEntities entities = new LittleGoatEntities())
                {
                    var serie = entities.Serie.FirstOrDefault(p => p.Id == key);
                    var players = serie.SeriePlayers.Select(p => p.Player.Name).ToArray();
                    var gameStarted = serie.Started;
                    return Json(new { players, gameStarted, stopCalls = false });
                }
            }
            catch
            {
                return Json(new
                {
                    players = Enumerable.Empty<Player>().ToArray(),
                    gameStarted = false,
                    stopCalls = true
                });
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [IsAuthenticated]
        public ActionResult NewGame(NewGameViewModel model)
        {
            var playerId = GetPlayerId();
            Random r = new Random();
            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var serie = entities.Serie.Single(p => p.Id == model.Key);
                if (serie.CreatorId != playerId)
                {
                    return RedirectToAction("NewGame", new { key = model.Key });
                }

                var players = serie.SeriePlayers.ToList();
                if (players.Count < 2)
                {
                    ViewBag.ErrorMessage = Resources.you_cant_play_alone;
                    model.IsCreator = serie.CreatorId == playerId;
                    return View(model);
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

            return RedirectToAction("Play", new { key = model.Key });
        }

        [IsAuthenticated]
        public ActionResult Play(string key)
        {
            var playerId = GetPlayerId();

            var model = new PlayViewModel();

            model.FirstTwoCards = new List<GameCard>();

            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var game = entities.Game.Single(p => p.SerieId == key && p.Ended == false);

                model.DateStopShowFirstTwoCards = game.CreationDate.AddSeconds(NB_SECONDS_TO_SHOW_FIRST_TWO_CARDS);

                if(game.CreationDate.AddSeconds(NB_SECONDS_TO_SHOW_FIRST_TWO_CARDS) >= DateTime.UtcNow)
                {
                    model.FirstTwoCards = entities.GameCard.Where(p => p.PlayerId == playerId).OrderBy(p => p.Position).Take(2).ToList();
                }
            }

            return View(model);
        }
    }
}