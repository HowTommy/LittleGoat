namespace Biquette.Controllers
{
    using Biquette.DataAccess;
    using Biquette.Filter;
    using Biquette.ViewModels;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class GameController : DefaultController
    {
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
                serie.Player1.Add(creator);

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
                if (playerId != null)
                {
                    player = entities.Player.FirstOrDefault(p => p.Id == playerId);
                    player.Name = model.Name;
                }
                else
                {
                    player = new Player() { Id = Guid.NewGuid().ToString(), Name = model.Name };
                    entities.Player.Add(player);
                    Response.Cookies["playerId"].Value = player.Id;
                    Response.Cookies["playerId"].Expires = DateTime.UtcNow.AddYears(10);
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
                var seriePlayers = serie.Player1.ToList();

                if(!seriePlayers.Any(p => p.Id == playerId))
                {
                    serie.Player1.Add(player);
                }

                entities.SaveChanges();

                return View(new NewGameViewModel() { Key = key, IsCreator = serie.CreatorId == playerId });
            }
        }

        [HttpPost]
        public ActionResult GetCurrentPlayers(string key)
        {
            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var players = entities.Serie.Single(p => p.Id == key).Player1.Select(p => p.Name).ToArray();
                return Json(players);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [IsAuthenticated]
        public ActionResult NewGame(NewGameViewModel model)
        {
            var playerId = GetPlayerId();
            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var serie = entities.Serie.Single(p => p.Id == model.Key);
                if(serie.CreatorId != playerId)
                {
                    return RedirectToAction("NewGame", new { key = model.Key });
                }

                serie.Started = true;

            }
            // lancer la partie
            throw new NotImplementedException();
        }
    }
}