namespace LittleGoat.Controllers
{
    using LittleGoat.DataAccess;
    using LittleGoat.ViewModels;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class AccountController : Controller
    {
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

            if (!string.IsNullOrWhiteSpace(model.Email) && !IsValidEmail(model.Email))
            {
                ViewBag.ErrorMessage = Resources.invalid_email;
                return View(model);
            }

            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                if (entities.Player.Any(p => p.Name.ToLower() == model.Name.ToLower()))
                {
                    ViewBag.ErrorMessage = Resources.name_already_used;
                    return View(model);
                }
                else if (!string.IsNullOrWhiteSpace(model.Email) && entities.Player.Any(p => p.Email.Trim().ToLowerInvariant() == model.Email.Trim().ToLowerInvariant()))
                {
                    ViewBag.ErrorMessage = Resources.email_already_used;
                    return View(model);
                }

                Player player;

                string playerId = Request.Cookies["playerId"]?.Value;
                if (playerId == null || entities.Player.FirstOrDefault(p => p.Id == playerId) == null)
                {
                    player = new Player() { Id = Guid.NewGuid().ToString(), Name = model.Name.Trim(), Email = model.Email?.Trim() };
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

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}