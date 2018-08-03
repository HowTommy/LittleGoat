namespace LittleGoat.Controllers
{
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

#if DEBUG
        public ActionResult CleanCookies()
        {
            CleanCookie();
            return RedirectToAction("Index");
        }
    }
#endif
}