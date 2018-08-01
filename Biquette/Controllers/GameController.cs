namespace Biquette.Controllers
{
    using System.Web.Mvc;

    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Create()
        {
            return View();
        }
    }
}