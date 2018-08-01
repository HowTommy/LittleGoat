namespace Biquette.Controllers
{
    using System;
    using System.Web.Mvc;

    public class DefaultController : Controller
    {
        protected string GetPlayerId()
        {
            return Request.Cookies["playerId"].Value;
        }

        protected void CleanCookie()
        {
            Response.Cookies["playerId"].Expires = DateTime.UtcNow.AddDays(-14);
        }
    }
}