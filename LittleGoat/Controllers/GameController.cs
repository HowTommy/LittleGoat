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