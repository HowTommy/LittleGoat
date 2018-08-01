namespace Biquette.Controllers
{
    using Biquette.DataAccess;
    using Biquette.ViewModels;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Create()
        {
            string playerId = Request.Cookies["playerId"]?.Value;

            if (string.IsNullOrWhiteSpace(playerId))
            {
                return RedirectToAction("ChooseName");
            }

            return RedirectToAction("NewGame", new { key = Guid.NewGuid().ToString() });
        }
        
        public ActionResult ChooseName(string returnUrl)
        {
            return View(model: returnUrl);
        }

        [HttpPost]
        public ActionResult ChooseName(ChooseNameViewModel model)
        {
            if(string.IsNullOrWhiteSpace(model.Name))
            {
                model.ErrorMessage = Resources.name_cant_be_empty;
                return View(model);
            }

            using(LittleGoatEntities entities = new LittleGoatEntities())
            {

            }
        }

        public ActionResult NewGame(string key)
        {
            string playerId = Request.Cookies["playerId"]?.Value;
            if (!string.IsNullOrEmpty(playerId))
            {
                playerId = Guid.NewGuid().ToString();
                Response.Cookies["playerId"].Value = playerId;
                Response.Cookies["playerId"].Expires = DateTime.UtcNow.AddYears(10);
            }

            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var existingPlayer = entities.Player.FirstOrDefault(p => p.Id == playerId);
                if (existingPlayer == null)
                {
                    entities.Player.Add(new Player() { Id = playerId, Name = "", SerieId = serieId });
                }
            }
            // sauvegarder le joueur (+ créer cookie) et créer la partie
            return View(new NewGameViewModel() { Key = key });
        }

        [HttpPost]
        public ActionResult NewGame(NewGameViewModel model)
        {
            //todo sauvegarder le nom dans un cookie
            // lancer la partie
            throw new NotImplementedException();
        }

        public ActionResult Join(string key)
        {
            // sauvegarder le joueur (+ créer cookie)
            return View();
        }
    }
}