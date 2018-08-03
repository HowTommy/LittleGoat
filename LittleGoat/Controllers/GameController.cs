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

    public class GameController : BaseController
    {
        private const int NB_SECONDS_TO_SHOW_FIRST_TWO_CARDS = 6;

        [ValidateAntiForgeryToken]
        [HttpPost]
        [IsAuthenticated]
        public ActionResult Launch(NewSerieViewModel model)
        {
            var playerId = GetPlayerId();
            Random r = new Random();
            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var serie = entities.Serie.Single(p => p.Id == model.Key);
                if (serie.CreatorId != playerId)
                {
                    return RedirectToAction("New", "Serie", new { key = model.Key });
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

            return RedirectToAction("Play", "Game", new { key = model.Key });
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

                if (game.CreationDate.AddSeconds(NB_SECONDS_TO_SHOW_FIRST_TWO_CARDS) >= DateTime.UtcNow)
                {
                    model.FirstTwoCards = entities.GameCard.Where(p => p.PlayerId == playerId).OrderBy(p => p.Position).Take(2).ToList();
                }

                if (game.NextToPlayId == playerId)
                {
                    model.CardFromDeck = entities.GameCard
                        .Where(p => p.GameId == game.Id && p.PlayerId == null)
                        .OrderBy(p => p.Position)
                        .FirstOrDefault();
                }
            }

            return View(model);
        }
    }
}